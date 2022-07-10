using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Controllers
{
    public class ProductWriteOffController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _unitOfWork;


        public ProductWriteOffController(DatabaseContext context, IEmailSender emailSender, IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork)
        {
            _context = context;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;

        }

        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.ProductWriteOff.Include(c => c.WriteOffReason).Include(b => b.product);
            return View(await databaseContext.ToListAsync());
        }



        public IActionResult WriteOff()
        {

            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["WriteOffReasonId"] = new SelectList(_context.WriteOffReason, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WriteOff([Bind("Id,WriteOffDate,writeOffQuantity, ProductId,writeOffReasonId")] WriteOffProduct productWriteOff)
        {
            if (ModelState.IsValid)
            {

                var item = _context.Products.Where(c => c.Id == productWriteOff.ProductId).FirstOrDefault();

                if (item.QuantityOnHand > productWriteOff.writeOffQuantity || item.QuantityOnHand == productWriteOff.writeOffQuantity)

                {
                    item.QuantityOnHand -= productWriteOff.writeOffQuantity;
                    _context.Add(productWriteOff);

                    TempData["success"] = "Product Written-Off Successfully";
                    await _context.SaveChangesAsync();

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var template = System.IO.File.ReadAllText(Path.Combine(wwwRootPath, @"emailTemp\lowProdTemp.html"));

                    var users = await (from user in _context.Users
                                       join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                       join role in _context.Roles on userRole.RoleId equals role.Id
                                       where role.Name == "Admin" || role.Name == "Inventory Manager"
                                       select user.Email)
                                       .ToListAsync();


                    string quantity = item.QuantityOnHand.ToString();
                    template = template.Replace("[ITEM]", item.Name).Replace("[QUANTITY]", quantity);


                    if (item.QuantityOnHand <= item.ThresholdValue && item.QuantityOnHand > 0)
                    {
                        template = template.Replace("[TEXT]", "LOW STOCK ALERT!");
                        string message = template;

                        foreach (var user in users)
                        {
                            await _emailSender.SendEmailAsync(
                                user,
                                "LOW STOCK ALERT",
                                message);
                        }
                    }
                    else if (item.QuantityOnHand == 0)
                    {
                        template = template.Replace("[TEXT]", "OUT OF STOCK ALERT!");
                        string message = template;

                        foreach (var user in users)
                        {
                            await _emailSender.SendEmailAsync(
                                user,
                                "OUT OF STOCK ALERT",
                                message);
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Error = "Quantity On Hand is less than the write-off value.";
                }




            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productWriteOff.ProductId);
            ViewData["WriteOffReasonId"] = new SelectList(_context.WriteOffReason, "Id", "Name", productWriteOff.writeOffReasonId);

            return View(productWriteOff);
        }


    }
}
