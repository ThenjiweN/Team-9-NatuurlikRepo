using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class InventoryWriteOffs : Controller
    {
        private readonly DatabaseContext _context;
        

        public InventoryWriteOffs(DatabaseContext context)
        {
            _context = context;
          
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
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.Error = "Quantity On Hand Is...";
                    }


               
                
            }
            ViewData["InventoryItemId"] = new SelectList(_context.InventoryItem, "Id", "InventoryItemName", inventoryWriteOff.InventoryItemId);
            ViewData["WriteOffReasonId"] = new SelectList(_context.WriteOffReason, "Id", "Name", inventoryWriteOff.writeOffReasonId);
         
            return View(inventoryWriteOff);
        }


    }
}
