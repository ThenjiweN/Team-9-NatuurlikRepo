using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class QueryReasonController : Controller
    {
        private readonly DatabaseContext _context;

        public QueryReasonController(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.QueryReason.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queryReason = await _context.QueryReason
                .FirstOrDefaultAsync(m => m.Id == id);
            if (queryReason == null)
            {
                return NotFound();
            }

            return View(queryReason);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] QueryReason queryReason
            )
        {
            if (ModelState.IsValid)
            {
                if (_context.QueryReason.Any(c => c.Name.Equals(queryReason.Name)))
                {
                    ViewBag.ReturnError = "Query Reason Already Exists!";
                }
                else
                {
                    _context.Add(queryReason);
                    _context.SaveChanges();
                    TempData["AlertMessage"] = "Query Reason Added Successflly!";
                    return RedirectToAction("Index");
                }

            }
            else if (!ModelState.IsValid)

            {
                ViewBag.modal = "Error! Please Try Again.";

            }
            return View(queryReason);
        }

        // GET: WriteOffReason/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queryReason = await _context.QueryReason.FindAsync(id);
            if (queryReason == null)
            {
                return NotFound();
            }
            return View(queryReason);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] QueryReason queryReason)
        {
            if (ModelState.IsValid)

            {

                if (_context.QueryReason.Any(c => c.Name.Equals(queryReason.Name)))
                {
                    ViewBag.ReturnError = "Query Reason Already Exists!";

                }
                else
                {
                    _context.Entry(queryReason).State = EntityState.Modified;
                    TempData["AlertMessage"] = "Query Reason Updated Successfully";
                    ViewBag.QueryReasonConfirmation = "Please confirm your changes";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(queryReason);
        }

        // GET: WriteOffReason/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var queryReason = await _context.QueryReason
                .FirstOrDefaultAsync(m => m.Id == id);
            if (queryReason == null)
            {
                return NotFound();
            }

            return View(queryReason);
        }

        // POST: WriteOffReason/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            QueryReason queryReason = _context.QueryReason.Find(id);
            _context.QueryReason.Remove(queryReason);
            ViewBag.QueryReasonConfirmation = "Are you sure you want to delete this query reason?";
            TempData["AlertMessage"] = "Query Reason Deleted Successfully";
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
