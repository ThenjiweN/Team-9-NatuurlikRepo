// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }


        public string Username { get; set; }


        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            [RegularExpression(@"^[ a-zA-Z ]+$", ErrorMessage = "Invalid First Name provided. No digits or special characters are allowed.")]
            [MaxLength(50)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }


            [Required]
            [RegularExpression(@"^[ a-zA-Z ]+$", ErrorMessage = "Invalid First Name provided. No digits or special characters are allowed.")]
            [MaxLength(50)]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Required]
            [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter a valid phone number.")]
            [Display(Name = "Phone Number")]
            [MaxLength(10)]
            public string PhoneNumber { get; set; }

            
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Street Address")]
            public string StreetAddress { get; set; }
            [Required]
            public int? Country { get; set; }
            [Required]
            public int? Province { get; set; }
            [Required]
            public int? Suburb { get; set; }
            [Required]
            public int? City { get; set; }

        
            [ValidateNever]
            public IEnumerable<SelectListItem> CountryList { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> ProvinceList { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> CityList { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> SuburbList { get; set; }



        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
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
                }),

                FirstName = user.FirstName,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                StreetAddress = user.StreetAddress,
                Country = user.CountryId,
                Province = user.ProvinceId,
                City = user.CityId,
                Suburb = user.SuburbId

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            else
            {

              await LoadAsync(user);
                return Page();
            }

          

            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.Surname != user.Surname)
            {
                user.Surname = Input.Surname;
            }

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = Input.PhoneNumber;
            }

            if (Input.StreetAddress != user.StreetAddress)
            {
                user.StreetAddress = Input.StreetAddress;
            }

            if (Input.Country != user.CountryId)
            {
                user.CountryId = Input.Country;
            }

            if (Input.Province != user.ProvinceId)
            {
                user.ProvinceId = Input.Province;
            }

            if (Input.City != user.CityId)
            {
                user.CityId = Input.City;
            }

            if (Input.Suburb != user.SuburbId)
            {
                user.SuburbId = Input.Suburb;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated successfully";
            return RedirectToPage();
        }
    }
}
