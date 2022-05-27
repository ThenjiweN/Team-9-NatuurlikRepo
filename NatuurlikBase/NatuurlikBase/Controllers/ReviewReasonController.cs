using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class ReviewReasonController : Controller
    {
        private readonly DatabaseContext _context;

        public ReviewReasonController(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.ReviewReason.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ReviewReason = await _context.ReviewReason
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ReviewReason == null)
            {
                return NotFound();
            }

            return View(ReviewReason);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] ReviewReason ReviewReason
            )
        {
            if (ModelState.IsValid)
            {
                if (_context.ReviewReason.Any(c => c.Name.Equals(ReviewReason.Name)))
                {
                    ViewBag.ReturnError = "Review Reason Already Exists!";
                }
                else
                {
                    _context.Add(ReviewReason);
                    _context.SaveChanges();
                    TempData["AlertMessage"] = "Review Reason Added Successflly!";
                    return RedirectToAction("Index");
                }

            }
            else if (!ModelState.IsValid)

            {
                ViewBag.modal = "Error! Please Try Again.";

            }
            return View(ReviewReason);
        }

        // GET: WriteOffReason/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ReviewReason = await _context.ReviewReason.FindAsync(id);
            if (ReviewReason == null)
            {
                return NotFound();
            }
            return View(ReviewReason);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] ReviewReason ReviewReason)
        {
            if (ModelState.IsValid)

            {

                if (_context.ReviewReason.Any(c => c.Name.Equals(ReviewReason.Name)))
                {
                    ViewBag.ReturnError = "Review Reason Already Exists!";

                }
                else
                {
                    _context.Entry(ReviewReason).State = EntityState.Modified;
                    TempData["AlertMessage"] = "Review Reason Updated Successfully";
                    ViewBag.ReviewReasonConfirmation = "Please confirm your changes";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(ReviewReason);
        }

        // GET: WriteOffReason/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ReviewReason = await _context.ReviewReason
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ReviewReason == null)
            {
                return NotFound();
            }

            return View(ReviewReason);
        }

        // POST: WriteOffReason/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ReviewReason ReviewReason = _context.ReviewReason.Find(id);
            _context.ReviewReason.Remove(ReviewReason);
            ViewBag.ReviewReasonConfirmation = "Are you sure you want to delete this Review reason?";
            TempData["AlertMessage"] = "Review Reason Deleted Successfully";
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