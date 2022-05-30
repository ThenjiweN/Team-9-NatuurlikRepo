using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class CourierController : Controller
    {
        private readonly DatabaseContext _context;

        public CourierController(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.Courier.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Courier
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courier == null)
            {
                return NotFound();
            }

            return View(courier);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,CourierName,CourierFee,EstimatedDeliveryTime")] Courier courier)
        {
            if (ModelState.IsValid)
            {
                if (_context.Courier.Any(c => c.CourierName == courier.CourierName && c.CourierFee == courier.CourierFee && c.EstimatedDeliveryTime == courier.EstimatedDeliveryTime))
                {
                    ViewBag.ReturnError = "Courier Already Exists!";
                }
                else
                {
                    _context.Add(courier);
                    _context.SaveChanges();
                    TempData["success"] = "Courier Added Successflly!";
                    return RedirectToAction("Index");
                }

            }
            else if (!ModelState.IsValid)

            {
                ViewBag.modal = "Error! Please Try Again.";

            }
            return View(courier);
        }

        // GET: WriteOffReason/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Courier.FindAsync(id);
            if (courier == null)
            {
                return NotFound();
            }
            return View(courier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CourierName,CourierFee,EstimatedDeliveryTime")] Courier courier)
        {
            if (ModelState.IsValid)

            {

                if (_context.Courier.Any(c => c.CourierName == courier.CourierName && c.CourierFee == courier.CourierFee && c.EstimatedDeliveryTime == courier.EstimatedDeliveryTime))
                {
                    ViewBag.ReturnError = "Courier Already Exists!";

                }
                else
                {
                    _context.Entry(courier).State = EntityState.Modified;
                    TempData["success"] = "Courier Updated Successfully";
                    ViewBag.CourierConfirmation = "Please confirm your changes";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(courier);
        }

        // GET: Courier/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courier = await _context.Courier
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courier == null)
            {
                return NotFound();
            }

            return View(courier);
        }

        // POST: WriteOffReason/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Courier courier = _context.Courier.Find(id);
            _context.Courier.Remove(courier);
            ViewBag.CourierConfirmation = "Are you sure you want to delete this courier";
            TempData["success"] = "Courier Deleted Successfully";
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}