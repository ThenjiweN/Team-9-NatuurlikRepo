using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Controllers
{
    public class QueryReasonController : Controller
    {
        private readonly DatabaseContext db;

        public QueryReasonController(DatabaseContext context)
        {
            db = context;
        }

        // GET: Countries
        public IActionResult Index()
        {
            return View(db.QueryReason.ToList());
        }

        // GET: Countries/Details/5


        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] QueryReason QueryReason)
        {
            if (ModelState.IsValid)

            {
                if (db.QueryReason.Any(c => c.Name.Equals(QueryReason.Name)))
                {
                    ViewBag.Error = "Query Reason Already exist in the database.";


                }
                else
                {
                    db.QueryReason.Add(QueryReason);

                    ViewBag.QueryReasonConfirmation = "Are you sure you want to add a Query reason.";
                    db.SaveChanges();

                    TempData["success"] = "Query Reason successfully added.";
                    TempData["NextCreation"] = "Query Reason Successfully Deleted.";

                    return RedirectToAction("Index");
                }

            }

            else if (!ModelState.IsValid)

            {
                ViewBag.modal = "invalid.";

            }
            return View(QueryReason);
        }


        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            QueryReason QueryReason = db.QueryReason.Find(id);
            if (QueryReason == null)
            {
                return NotFound();
            }
            return View(QueryReason);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,Name")] QueryReason QueryReason)
        {
            if (ModelState.IsValid)

            {

                if (db.QueryReason.Any(c => c.Name.Equals(QueryReason.Name)))
                {
                    ViewBag.Error = "Query Reason Already exist in the database.";

                }
                else
                {
                    db.Entry(QueryReason).State = EntityState.Modified;
                    TempData["success"] = "Query Reason Successfully Edited.";
                    ViewBag.QueryReasonConfirmation = "Are you sure with your Query reason changes.";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(QueryReason);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }
            QueryReason QueryReason = db.QueryReason.Find(id);
            if (QueryReason == null)
            {
                return NotFound();
            }
            return View(QueryReason);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            QueryReason queryReason = db.QueryReason.Find(id);
            db.QueryReason.Remove(queryReason);
            ViewBag.QueryReasonConfirmation = "Are you sure you want to delete this query reason?";
            TempData["AlertMessage"] = "Query Reason Deleted Successfully";
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
