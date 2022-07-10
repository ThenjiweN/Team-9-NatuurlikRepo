using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class VATController : Controller
    {

        private readonly DatabaseContext db;

        public VATController(DatabaseContext context)
        {
            db = context;
        }

        // GET: Countries
        public IActionResult Index()
        {
            var databaseContext = db.VAT;
            return View(databaseContext.ToList());
        }


        public IActionResult Create()
        {
            //ViewData["InventoryTypeId"] = new SelectList(db.InventoryType, "Id", "InventoryTypeName");

            //ViewData["VATStatus"] = new SelectList(db.InventoryType, "Id", "InventoryTypeName");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,VATPercentage,VATFactor,VATStatus,CreatedDate")] VAT vat)
        {
            if (ModelState.IsValid)
            {
                //Check that only one VAT instance is active at all times and no duplicates exist.
                if (db.VAT.Any(c => c.VATPercentage == vat.VATPercentage))
                {
                    ViewBag.Error = "VAT Details Already Exists.";
                }

                else if (db.VAT.Any(c => c.VATStatus == vat.VATStatus && vat.VATStatus == "Active"))
                {
                    
                        ViewBag.Error = "Only one VAT instance may be active at all times.";
                }

                else
                {
                    var vatFactor = Convert.ToDecimal(vat.VATPercentage / 100.00) ;
                    vat.VATFactor = vatFactor;
                    db.VAT.Add(vat);
                    db.SaveChanges();
                    TempData["success"] = "VAT Details Created Successfully.";
                    return RedirectToAction("Index");
                }

            }

            return View(vat);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VAT vatFromDb = db.VAT.Find(id);
            if (vatFromDb == null)
            {
                return NotFound();
            }

            return View(vatFromDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,VATPercentage,VATFactor,VATStatus,CreatedDate")] VAT vat)
        {
            if (ModelState.IsValid)
            {
                //check if other Status = Active

                if (db.VAT.Any(c => c.VATStatus == "Active" && c.VATStatus == vat.VATStatus))
                {
                    ViewBag.Error = "Another VAT instance is already Active.";
                }
               
                else
                {
                    var vatFactor = Convert.ToDecimal(vat.VATPercentage / 100.00);
                    vat.VATFactor = vatFactor;
                    db.Entry(vat).State = EntityState.Modified;
                    TempData["success"] = "VAT Details Updated Successfully.";
                    ViewBag.Prompt = "Are you sure you wish to save the changes.";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(vat);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }
            var vatInstance = db.VAT.FirstOrDefault(m => m.Id == id);


            if (vatInstance == null)
            {
                return NotFound();
            }
            return View(vatInstance);

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            VAT vatInstance = db.VAT.Find(id);
            db.VAT.Remove(vatInstance);
            ViewBag.Confirmation = "Are you sure you want to proceed with removal?";
            TempData["success"] = "VAT Details Successfully Deleted.";
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
