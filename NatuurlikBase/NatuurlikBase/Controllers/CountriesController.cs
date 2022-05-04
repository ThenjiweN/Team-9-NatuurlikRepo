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
    public class CountriesController : Controller
    {
        private readonly DatabaseContext db;

        public CountriesController(DatabaseContext context)
        {
            db = context;
        }

        // GET: Countries
        public IActionResult Index()
        {
            return View(db.Country.ToList());
        }

        // GET: Countries/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Country country = db.Country.Find(id);
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
        public IActionResult Create([Bind("ID,CountryName")] Country country)
        {
            if (ModelState.IsValid)

            {
                if (db.Country.Any(c => c.CountryName.Equals(country.CountryName)))
                {
                    ViewBag.CountryError = "Country name Already exist in the database.";

                    TempData["AlertMessages"] = "Country name Already exist in the database.";
                }
                else
                {
                    db.Country.Add(country);

                    ViewBag.CountryConfirmation = "Are you sure you want to add a country.";
                    db.SaveChanges();

                    TempData["AlertMessage"] = "Country name successfully added.";
                    return RedirectToAction("Index");
                }

            }

            else if (!ModelState.IsValid)

            {
                ViewBag.modal = "invalid.";

            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Country country = db.Country.Find(id);
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
        public IActionResult Edit([Bind("Id,CountryName")] Country country)
        {
            if (ModelState.IsValid)

            {

                if (db.Country.Any(c => c.CountryName.Equals(country.CountryName)))
                {
                    ViewBag.CountryError = "Country name Already exist in the database.";

                }
                else
                {
                    db.Entry(country).State = EntityState.Modified;
                    TempData["AlertMessage"] = "Country name successfully Edited.";
                    ViewBag.CountryConfirmation = "Are you sure with your country name changes.";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }
            Country country = db.Country.Find(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);

        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Country country = db.Country.Find(id);
            db.Country.Remove(country);
            ViewBag.CountryConfirmation = "Are you sure you want to delete a country.";
            TempData["AlertMessage"] = "Country name successfully Deleted.";
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
