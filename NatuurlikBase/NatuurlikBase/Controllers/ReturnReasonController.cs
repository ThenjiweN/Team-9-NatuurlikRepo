#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class ReturnReasonController : Controller
    {
        private readonly DatabaseContext db;

        public ReturnReasonController(DatabaseContext context)
        {
            db = context;
        }

        // GET: Countries
        public IActionResult Index()
        {
            return View(db.ReturnReason.ToList());
        }

        // GET: Countries/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ReturnReason country = db.ReturnReason.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ReturnReasonName")] ReturnReason returnReason)
        {
            if (ModelState.IsValid)

            {
                if (db.ReturnReason.Any(c => c.ReturnReasonName.Equals(returnReason.ReturnReasonName)))
                {
                    ViewBag.ReturnError = "Return reason Already exist in the database.";

                }
                else
                {
                    db.ReturnReason.Add(returnReason);

                    ViewBag.CountryConfirmation = "Are you sure you want to add a return reason.";
                    db.SaveChanges();

                    TempData["success"] = "Return reason successfully added.";
                    return RedirectToAction("Index");
                }

            }

            else if (!ModelState.IsValid)

            {
                ViewBag.modal = "invalid.";

            }
            return View(returnReason);
        }

        // GET: Countries/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ReturnReason country = db.ReturnReason.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,ReturnReasonName")] ReturnReason returnReason)
        {
            if (ModelState.IsValid)

            {

                if (db.ReturnReason.Any(c => c.ReturnReasonName.Equals(returnReason.ReturnReasonName)))
                {
                    ViewBag.ReturnError = "Return Reason Already exist in the database.";

                }
                else
                {
                    db.Entry(returnReason).State = EntityState.Modified;
                    TempData["success"] = "Return reason successfully Updated.";
                    ViewBag.ReturnReasonConfirmation = "Are you sure with your return reason changes.";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(returnReason);
        }

        // GET: Countries/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }
            ReturnReason returnReason = db.ReturnReason.Find(id);
            if (returnReason == null)
            {
                return NotFound();
            }
            return View(returnReason);

        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            ReturnReason country = db.ReturnReason.Find(id);
            db.ReturnReason.Remove(country);
            ViewBag.CountryConfirmation = "Are you sure you want to delete a country.";
            TempData["success"] = "Return Reason successfully Deleted.";
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

