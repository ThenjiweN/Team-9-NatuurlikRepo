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
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Controllers
{
    public class CitiesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CitiesController(DatabaseContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;   
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.City.Include(c => c.Province);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .Include(c => c.Province)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "ProvinceName");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CityName,ProvinceId")] City city)
        {
            if (ModelState.IsValid)
            {

                if (_context.City.Any(c => c.CityName.Equals(city.CityName)))
                {
                    ViewBag.Error = "City Already Exist In The Database.";
                  
                }
                else
                {
                    _context.Add(city);
                    TempData["success"] = "City Created Successfully";
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "ProvinceName", city.ProvinceId);
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "ProvinceName", city.ProvinceId);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityName,ProvinceId")] City city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.City.Any(c => c.CityName.Equals(city.CityName)))
                {
                    ViewBag.Error = "City Already Exist In The Database.";
                    ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "CountryName", city.ProvinceId);
                }
                else
                {
                    try
                    {
                        _context.Update(city);
                        TempData["success"] = "City Updated Successfully";
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CityExists(city.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "ProvinceName", city.ProvinceId);
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .Include(c => c.Province)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            City city = _context.City.Find(id);
            _context.City.Remove(city);
            ViewBag.CountryConfirmation = "Are you sure you want to delete this city?";

            var hasFk = _unitOfWork.Suburb.GetAll().Any(x => x.CityId == id);

            if (!hasFk)
            {
                var obj = _unitOfWork.City.GetFirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    TempData["AlertMessage"] = "Error occurred while attempting delete";
                }
                _unitOfWork.City.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "City Successfully Deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = "City cannot be deleted since it has a Suburb associated";
                return RedirectToAction("Index");
            }
        }

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
