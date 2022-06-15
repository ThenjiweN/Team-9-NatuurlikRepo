using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Controllers
{
    public class InventoryTypeController : Controller
    {
        private readonly DatabaseContext db;

        public InventoryTypeController(DatabaseContext context)
        {
            db = context;
        }

        // GET: Countries
        public IActionResult Index()
        {
            return View(db.InventoryType.ToList());
        }

        // GET: Countries/Details/5
   

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,InventoryTypeName")] InventoryType inventoryType)
        {
            if (ModelState.IsValid)

            {
                if (db.InventoryType.Any(c => c.InventoryTypeName.Equals(inventoryType.InventoryTypeName)))
                {
                    ViewBag.Error = "Inventory Type Already exist in the database.";


                }
                else
                {
                    db.InventoryType.Add(inventoryType);

                    ViewBag.CountryConfirmation = "Are you sure you want to add a return reason.";
                    db.SaveChanges();

                    TempData["success"] = "Inventory Type successfully added.";
                    TempData["NextCreation"] = "Inventory Type Successfully Deleted.";

                    return RedirectToAction("Index");
                }

            }

            else if (!ModelState.IsValid)

            {
                ViewBag.modal = "invalid.";

            }
            return View(inventoryType);
        }

    
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            InventoryType inventoryType = db.InventoryType.Find(id);
            if (inventoryType == null)
            {
                return NotFound();
            }
            return View(inventoryType);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,InventoryTypeName")] InventoryType inventoryType)
        {
            if (ModelState.IsValid)

            {

                if (db.InventoryType.Any(c => c.InventoryTypeName.Equals(inventoryType.InventoryTypeName)))
                {
                    ViewBag.Error = "Return Reason Already exist in the database.";

                }
                else
                {
                    db.Entry(inventoryType).State = EntityState.Modified;
                    TempData["success"] = "Inventory Type Successfully Updated.";
                    ViewBag.ReturnReasonConfirmation = "Are you sure with your return reason changes.";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(inventoryType);
        }

      
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }
            InventoryType inventoryType = db.InventoryType.Find(id);
            if (inventoryType == null)
            {
                return NotFound();
            }
            return View(inventoryType);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            InventoryType inventoryType = db.InventoryType.Find(id);
            db.InventoryType.Remove(inventoryType);

            ViewBag.CountryConfirmation = "Are you sure you want to delete a country.";
            var hasFk =db.InventoryItem.Any(c => c.InventoryTypeId == id);

            if (!hasFk)
            {
                var obj = db.InventoryType.FirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    TempData["AlertMessage"] = "Error occurred while attempting delete";
                }
                db.InventoryType.Remove(obj);
                TempData["success"] = "Inventory Type Successfully Deleted.";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Delete"] = "Inventory Type cannot be deleted since it has an Inventory Item associated";
                return RedirectToAction("Index");
            }
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

