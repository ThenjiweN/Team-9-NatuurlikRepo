using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Controllers
{
    public class InventoryWriteOffs : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public InventoryWriteOffs(DatabaseContext context, IEmailSender emailSender, IWebHostEnvironment hostEnvironment, IUnitOfWork unitOfWork)
        {
            _context = context;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.InventoryWriteOff.Include(c => c.WriteOffReason).Include (b=> b.InventoryItem);
            return View(await databaseContext.ToListAsync());
        }

    
        
        public IActionResult WriteOff()
        {
            
            ViewData["InventoryItemId"] = new SelectList(_context.InventoryItem, "Id", "InventoryItemName");
            ViewData["WriteOffReasonId"] = new SelectList(_context.WriteOffReason, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WriteOff([Bind("Id,WriteOffDate,writeOffQuantity, InventoryItemId,writeOffReasonId")] WriteOffInventory inventoryWriteOff)
        {
            if (ModelState.IsValid)
            {

                var item = _context.InventoryItem.Where(c => c.Id == inventoryWriteOff.InventoryItemId).FirstOrDefault();

                if (item.QuantityOnHand > inventoryWriteOff.writeOffQuantity || item.QuantityOnHand == inventoryWriteOff.writeOffQuantity)
                {
                    item.QuantityOnHand -= inventoryWriteOff.writeOffQuantity;
                    //  item.QuantityOnHand = item.QuantityOnHand-inventoryWriteOff.writeOffQuantity;
                    _context.Add(inventoryWriteOff);
                    TempData["success"] = "Inventory Item Written-Off Successfully";
                    await _context.SaveChangesAsync();

                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    var template = System.IO.File.ReadAllText(Path.Combine(wwwRootPath, @"emailTemp\lowInvTemp.html"));

                    var users = await (from user in _context.Users
                                       join userRole in _context.UserRoles on user.Id equals userRole.UserId
                                       join role in _context.Roles on userRole.RoleId equals role.Id
                                       where role.Name == "Admin" || role.Name == "Inventory Manager"
                                       select user.Email)
                                       .ToListAsync();


                    string quantity = item.QuantityOnHand.ToString();
                    template = template.Replace("[ITEM]", item.InventoryItemName).Replace("[QUANTITY]", quantity);


                    if (item.QuantityOnHand <= item.ThresholdValue && item.QuantityOnHand > 0)
                    {
                        template = template.Replace("[TEXT]", "LOW INVENTORY ALERT!");
                        string message = template;

                        foreach (var user in users)
                        {
                            await _emailSender.SendEmailAsync(
                                user,
                                "LOW INVENTORY ALERT",
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
            ViewData["InventoryItemId"] = new SelectList(_context.InventoryItem, "Id", "InventoryItemName", inventoryWriteOff.InventoryItemId);
            ViewData["WriteOffReasonId"] = new SelectList(_context.WriteOffReason, "Id", "Name", inventoryWriteOff.writeOffReasonId);
         
            return View(inventoryWriteOff);
        }
    }
}
