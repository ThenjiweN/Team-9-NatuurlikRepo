using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Controllers
{
    public class InventoryItemController : Controller
    {

      private readonly DatabaseContext db;

        public InventoryItemController(DatabaseContext context)
        {
            db = context;
        }

        // GET: Countries
        public IActionResult Index()
        {
            var databaseContext = db.InventoryItem.Include(c => c.InventoryType);
            return View( databaseContext.ToList()); 
        }

     
        public IActionResult Create()
        {
            ViewData["InventoryTypeId"] = new SelectList(db.InventoryType, "Id", "InventoryTypeName");
      
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,InventoryItemName,InventoryTypeId,QuantityOnHand,ThresholdValue")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)

            {

                if (db.InventoryItem.Any(c => c.InventoryItemName == inventoryItem.InventoryItemName && c.InventoryTypeId == inventoryItem.InventoryTypeId && c.QuantityOnHand == inventoryItem.QuantityOnHand
                && c.ThresholdValue == inventoryItem.ThresholdValue))
                {
                    ViewBag.Error = "Inventory Type Already exist in the database.";
                }
                else
                {
                    db.InventoryItem.Add(inventoryItem);

                    ViewBag.CountryConfirmation = "Are you sure you want to add a return reason.";
                    db.SaveChanges();

                    TempData["success"] = "Inventory Item successfully added.";
                    return RedirectToAction("Index");
                }

            }

            ViewData["InventoryTypeId"] = new SelectList(db.InventoryType, "Id", "InventoryTypeName", inventoryItem.InventoryTypeId);
            return View(inventoryItem);
        }

     
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            InventoryItem inventoryItem = db.InventoryItem.Find(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            ViewData["InventoryTypeId"] = new SelectList(db.InventoryType, "Id", "InventoryTypeName", inventoryItem.InventoryTypeId);
        
            return View(inventoryItem);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,InventoryItemName,InventoryTypeId,QuantityOnHand,ThresholdValue")] InventoryItem inventoryItem)
        {
            if (ModelState.IsValid)

            {
                    if (db.InventoryItem.Any(c => c.InventoryItemName==inventoryItem.InventoryItemName &&c.InventoryTypeId==inventoryItem.InventoryTypeId && c.QuantityOnHand==inventoryItem.QuantityOnHand
                    && c.ThresholdValue == inventoryItem.ThresholdValue))
                {
                    ViewBag.Error = "Already exist in the database.";

                }
                else
                {
                    db.Entry(inventoryItem).State = EntityState.Modified;
                    TempData["success"] = "Inventory Item Successfully Updated.";
                    ViewBag.ReturnReasonConfirmation = "Are you sure with your return reason changes.";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewData["InventoryTypeId"] = new SelectList(db.InventoryType, "Id", "InventoryTypeName", inventoryItem.InventoryTypeId);
            return View(inventoryItem);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }
         //   InventoryItem inventoryItem = db.InventoryItem.Find(id);
            var inventoryItem =db.InventoryItem
             .Include(c => c.InventoryType)
             .FirstOrDefault(m => m.Id == id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            return View(inventoryItem);

        }

   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            InventoryItem inventoryItem = db.InventoryItem.Find(id);
            db.InventoryItem.Remove(inventoryItem);
            ViewBag.CountryConfirmation = "Are you sure you want to delete a country.";
            TempData["success"] = "Inventory Item Successfully Deleted.";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

