// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Areas.Identity.Pages.Account
{
    public class RegisterUserModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public RegisterUserModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public string ReturnUrl { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "First Name")]
            [RegularExpression(@"^[ a-zA-Z ]+$", ErrorMessage = "Invalid First Name provided. No digits or special characters are allowed.")]
            [MaxLength(50)]
            public string FirstName { get; set; }

            [Required]
            [RegularExpression(@"^[ a-zA-Z ]+$", ErrorMessage = "Invalid Surname provided. No digits or special characters are allowed.")]
            [MaxLength(50)]
            public string Surname { get; set; }
            [Required]
            [Display(Name = "Street Address")]
            public string StreetAddress { get; set; }
            [Required]
            public int Country { get; set; }
            [Required]
            public int Province { get; set; }
            [Required]

            public int Suburb { get; set; }
            [Required]
            public int City { get; set; }

            [Required]
            [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter a valid Phone Number.")]
            [Display(Name = "Phone Number")]
            [MaxLength(10)]
            public string PhoneNumber { get; set; }
            public string? Role { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> CountryList { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> ProvinceList { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> CityList { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> SuburbList { get; set; }


            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

           
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!_roleManager.RoleExistsAsync(SR.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SR.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SR.Role_MD)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SR.Role_SA)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SR.Role_IM)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SR.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SR.Role_Reseller)).GetAwaiter().GetResult();
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Input = new InputModel()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),

                //Cascade Droplists
                CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CountryName,
                    Value = u.Id.ToString()
                }),
                //ProvinceList = _unitOfWork.Province.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.ProvinceName,
                //    Value = u.Id.ToString()
                //}),
                //CityList = _unitOfWork.City.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.CityName,
                //    Value = u.Id.ToString()
                //}),
                //SuburbList = _unitOfWork.Suburb.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.SuburbName,
                //    Value = u.Id.ToString()
                //})

            };
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser(Input);
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (Input.Role == null)
                    {
                        //how a customer will be registered on the system.
                        await _userManager.AddToRoleAsync(user, SR.Role_Customer);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var template = System.IO.File.ReadAllText(Path.Combine(wwwRootPath, @"emailTemp\regUserTemp.html"));
                    template = template.Replace("[URL]", $"{HtmlEncoder.Default.Encode(callbackUrl)}");
                    string message = template;

                    await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Confirm your Natuurlik Account",
                    message);


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        TempData["success"] = "User registered successfully.";
                        return RedirectToAction("Index", "User");

                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    Input = new InputModel()
                    {
                        RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                        {
                            Text = i,
                            Value = i
                        }),
                        CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                        {
                            Text = u.CountryName,
                            Value = u.Id.ToString()
                        }),
                        ProvinceList = _unitOfWork.Province.GetAll().Select(u => new SelectListItem
                        {
                            Text = u.ProvinceName,
                            Value = u.Id.ToString()
                        }),
                        CityList = _unitOfWork.City.GetAll().Select(u => new SelectListItem
                        {
                            Text = u.CityName,
                            Value = u.Id.ToString()
                        }),
                        SuburbList = _unitOfWork.Suburb.GetAll().Select(u => new SelectListItem
                        {
                            Text = u.SuburbName,
                            Value = u.Id.ToString()
                        })

                    };
                }
            }
            return Page();
        }

        private ApplicationUser CreateUser(InputModel Input)
        {
            var user = new ApplicationUser
            {
                FirstName = Input.FirstName,
                Surname = Input.Surname,
                PhoneNumber = Input.PhoneNumber,
                StreetAddress = Input.StreetAddress,
                CountryId = Input.Country,
                ProvinceId = Input.Province,
                CityId = Input.City,
                SuburbId = Input.Suburb
            };
            _userManager.CreateAsync(user);
            return user;
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
