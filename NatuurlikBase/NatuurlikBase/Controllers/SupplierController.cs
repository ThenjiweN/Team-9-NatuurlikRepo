using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace NatuurlikBase.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _db;
        public SupplierController(IUnitOfWork unitOfWork, DatabaseContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Supplier> supplierList = _unitOfWork.Supplier.GetAll();
            return View(supplierList);
        }

        public ActionResult GetProvince(int countryId)
        {
            return Json(_db.Province.Where(x => x.CountryId == countryId).Select(x => new
            {
                Text = x.ProvinceName,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }


        [HttpGet]
        public ActionResult GetCity(int provinceId)
        {
            return Json(_db.City.Where(x => x.ProvinceId == provinceId).Select(x => new
            {
                Text = x.CityName,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }

        [HttpGet]
        public ActionResult GetSuburb(int cityId)
        {
            return Json(_db.Suburb.Where(x => x.CityId == cityId).Select(x => new
            {
                Text = x.SuburbName,
                Value = x.Id
            }).OrderBy(x => x.Text).ToList());
        }

        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_db.Country, "Id", "CountryName");
            ViewData["ProvinceId"] = new SelectList(_db.Province, "Id", "ProvinceName");
            ViewData["CityId"] = new SelectList(_db.City, "Id", "CityName");
            ViewData["SuburbId"] = new SelectList(_db.Suburb, "Id", "SuburbName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier obj)
        {
            if (ModelState.IsValid)
            {
                if (_db.Suppliers.Any(c => c.CompanyName == obj.CompanyName && c.EmailAddress == obj.EmailAddress))
                {
                    ViewBag.Error = "Supplier Already Exists.";
                }
                else
                {
                    _unitOfWork.Supplier.Add(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "Supplier created successfully";
                    return RedirectToAction("Index");
                }
            }
            ViewData["CountryId"] = new SelectList(_db.Country, "Id", "CountryName", obj.CountryId);
            ViewData["ProvinceId"] = new SelectList(_db.Province, "Id", "ProvinceName", obj.ProvinceId);
            ViewData["CityId"] = new SelectList(_db.City, "Id", "CityName", obj.CityId);
            ViewData["SuburbId"] = new SelectList(_db.Suburb, "Id", "SuburbName", obj.SuburbId);
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var supplier = _unitOfWork.Supplier.GetFirstOrDefault(u => u.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_db.Country, "Id", "CountryName", supplier.CountryId);
            ViewData["ProvinceId"] = new SelectList(_db.Province, "Id", "ProvinceName", supplier.ProvinceId);
            ViewData["CityId"] = new SelectList(_db.City, "Id", "CityName", supplier.CityId);
            ViewData["SuburbId"] = new SelectList(_db.Suburb, "Id", "SuburbName", supplier.SuburbId);
            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Supplier obj)
        {

            if (ModelState.IsValid)
            {

                if (_db.Suppliers.Any(c => c.CompanyName == obj.CompanyName && c.PhoneNumber == obj.PhoneNumber
                && c.EmailAddress == obj.EmailAddress && c.StreetAddress == obj.StreetAddress && c.CountryId == obj.CountryId && c.ProvinceId == obj.ProvinceId
                && c.CityId == obj.CityId && c.SuburbId == obj.SuburbId))
                {
                    ViewBag.Error = "Supplier Already Exists.";
                }
                else
                {
                    _unitOfWork.Supplier.Update(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "Supplier Updated Successfully";
                    return RedirectToAction("Index");
                }
            }
            ViewData["CountryId"] = new SelectList(_db.Country, "Id", "CountryName", obj.CountryId);
            ViewData["ProvinceId"] = new SelectList(_db.Province, "Id", "ProvinceName", obj.ProvinceId);
            ViewData["CityId"] = new SelectList(_db.City, "Id", "CityName", obj.CityId);
            ViewData["SuburbId"] = new SelectList(_db.Suburb, "Id", "SuburbName", obj.SuburbId);
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _db.Suppliers
                .Include(c => c.Province)
                .Include(c => c.City)
                .Include(c => c.Suburb)
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            ViewBag.CountryConfirmation = "Are you sure you want to delete this Supplier?";

            var supplier = _unitOfWork.Supplier.GetFirstOrDefault(u => u.Id == id);
            if (supplier == null)
            {
                TempData["AlertMessage"] = "Error occurred while attempting delete";
            }
            _unitOfWork.Supplier.Remove(supplier);
            _unitOfWork.Save();
            TempData["success"] = "Supplier Deleted Successfully.";
            return RedirectToAction("Index");

        }



        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeletePOST(int? id)
        //{
        //    ViewBag.CountryConfirmation = "Are you sure you want to delete this Supplier?";

        //    var hasFk = _unitOfWork.Supplier.GetAll().Any(x => x.Id == id);

        //    if (!hasFk)
        //    {
        //        var supplier = _unitOfWork.Supplier.GetFirstOrDefault(u => u.Id == id);
        //        if (supplier == null)
        //        {
        //            TempData["AlertMessage"] = "Error occurred while attempting delete";
        //        }
        //        _unitOfWork.Supplier.Remove(supplier);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Supplier Successfully Deleted.";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        TempData["success"] = "Supplier cannot be deleted";
        //        return RedirectToAction("Index");
        //    }
        //}

    }
}
