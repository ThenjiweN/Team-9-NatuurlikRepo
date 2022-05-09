using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;

namespace NatuurlikBase.Controllers
{
    public class ProvinceController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _db;

        public ProvinceController(IUnitOfWork unitOfWork, DatabaseContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        // GET: Cities
        public async Task<IActionResult> Index()
        {
            var databaseContext = _db.Province.Include(c => c.Country);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _db.Province
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_db.Country, "Id", "CountryName");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProvinceName,CountryId")] Province province)
        {
            if (ModelState.IsValid)
            {
                if (_db.Province.Any(c => c.ProvinceName.Equals(province.ProvinceName)))
                {
                    ViewBag.Error = "Province Name Already Exist In The Database.";

                }
                else
                {
                   
                    _db.Province.Add(province);
                    await _db.SaveChangesAsync();
                    TempData["success"] = "Province Created Successfully";
                    return RedirectToAction(nameof(Index));
                  
                }
               
            }
            ViewData["CountryId"] = new SelectList(_db.Province, "Id", "CountryName", province.CountryId);
            return View(province);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _db.Province.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_db.Country, "Id", "CountryName", province.CountryId);
            return View(province);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProvinceName,CountryId")] Province province)
        {
            if (id != province.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_db.Province.Any(c => c.ProvinceName.Equals(province.ProvinceName)))
                {
                    ViewBag.Error = "Province Name Already Exist In The Database.";
                    ViewData["CountryId"] = new SelectList(_db.Province, "Id", "CountryName", province.CountryId);
                }
                else { 
                
                {
                        _db.Entry(province).State = EntityState.Modified;
                        
                        TempData["success"] = "Province Updated Successfully";
                        await _db.SaveChangesAsync();
                    }
               
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_db.Province, "Id", "CountryName", province.CountryId);
            return View(province);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = _db.Province
                .Include(c => c.Country)
                .FirstOrDefault(m => m.Id == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Province province = _db.Province.Find(id);
            _db.Province.Remove(province);
            ViewBag.CountryConfirmation = "Are you sure you want to delete this province?";

            var hasFk = _unitOfWork.City.GetAll().Any(x => x.ProvinceId == id);

            if (!hasFk)
            {
                var obj = _unitOfWork.Province.GetFirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    TempData["AlertMessage"] = "Error occurred while attempting delete";
                }
                _unitOfWork.Province.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Province Successfully Deleted.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["success"] = "Province cannot be deleted since it has a City associated";
                return RedirectToAction("Index");
            }
        }

        private bool ProvinceExists(int id)
        {
            return _db.Province.Any(e => e.Id == id);
        }
    }

}

