using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index()
        {
            //IList<Brand> objList = _unitOfWork.Brand.GetAll();
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            //Product product = new();
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text=u.CategoryName,
            //        Value= u.Id.ToString()
            //    });

            //IEnumerable<SelectListItem> BrandsList = _unitOfWork.Brand.GetAll().Select(
            //   u => new SelectListItem
            //   {
            //       Text = u.Name,
            //       Value = u.Id.ToString()
            //   });

            ProvinceVM provinceVM = new()
            {
                Province = new(),
                CountryList = _unitOfWork.Country.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CountryName,
                    Value = i.Id.ToString()
                })
            };

        

            if (id== null || id == 0)
            {
                //Create the product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["BrandsList"] = BrandsList;
                return View(provinceVM);
            }
            else
            {
                //update the product
                provinceVM.Province = _unitOfWork.Province.GetFirstOrDefault(u => u.Id == id);
                return View(provinceVM);
            }
      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //protect against anti forgery token attacks.
        public IActionResult Upsert(ProvinceVM obj)
        {
         
            if(ModelState.IsValid)
            {

              
                if(obj.Province.Id == 0)
                {
                    _unitOfWork.Province.Add(obj.Province);
                    TempData["success"] = "Province Created Successfully!";
                }
                else
                {
                    _unitOfWork.Province.Update(obj.Province);
                    TempData["success"] = "Province Updated Successfully!";
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
            var objList = _unitOfWork.Province.GetAll(includeProperties: "Country");
            return Json(new {data = objList });
        }
        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {

            var hasFk = _unitOfWork.City.GetAll().Any(x => x.Id == id);

            if (!hasFk)
            {
                var obj = _unitOfWork.Province.GetFirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    return Json(new { success = false, message = "An error occured while deleting" });
                }
                _unitOfWork.Province.Remove(obj);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Province Deleted Successfully" });
            }

            return Ok(TempData["error"] = "Province cannot be deleted since it has a City associated");

        }


       

        #endregion

    }

}

