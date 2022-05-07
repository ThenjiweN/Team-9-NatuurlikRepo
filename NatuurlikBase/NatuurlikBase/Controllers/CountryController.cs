using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;

namespace NatuurlikBase.Controllers
{
    public class CountryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _db;

        public CountryController(IUnitOfWork unitOfWork, DatabaseContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }
        public IActionResult Index()
        {

            return View();
     
        }
        //GET
        public IActionResult Upsert(int? id)
        {
           

            CountryVM countryVM = new()
            {
                Country = new(),
                
            };



            if (id == null || id == 0)
            {
                
                return View(countryVM);
            }
            else
            {
                //update the product
                countryVM.Country = _unitOfWork.Country.GetFirstOrDefault(u => u.Id == id);
                return View(countryVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //protect against anti forgery token attacks.
        public IActionResult Upsert(CountryVM obj)
        {
          
            if (ModelState.IsValid)
            {


                if (obj.Country.Id == 0)
                {
                    _unitOfWork.Country.Add(obj.Country);
                    TempData["success"] = "Country Created Successfully!";
                }
                else
                {
                    _unitOfWork.Country.Update(obj.Country);
                    TempData["success"] = "Country Updated Successfully!";
                }

                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);

        }






        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var objList = _unitOfWork.Country.GetAll(includeProperties: "");
            return Json(new { data = objList });
        }
        [HttpDelete]

        public IActionResult Delete(int? id)
        {

            var hasFk = _unitOfWork.Province.GetAll().Any(x => x.CountryId == id);

            if (!hasFk)
            {
                var obj = _unitOfWork.Country.GetFirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    return Json(new { success = false, message = "An error occured while deleting" });
                }
                _unitOfWork.Country.Remove(obj);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Country Deleted Successfully" });
            }

            return Ok(TempData["error"] = "Country cannot be deleted since it has a Province associated"); 




        }
        #endregion

    }

}

