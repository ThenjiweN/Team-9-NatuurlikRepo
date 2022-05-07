using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;

namespace NatuurlikBase.Controllers
{
    public class CityController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        //GET
        public IActionResult Upsert(int? id)
        {

            CityVM cityVM = new()
            {
                City = new(),
                ProvinceList = _unitOfWork.Province.GetAll().Select(i => new SelectListItem
                {
                    Text = i.ProvinceName,
                    Value = i.Id.ToString()
                })
            };

        

            if (id== null || id == 0)
            {
             
                return View(cityVM);
            }
            else
            {
                //update the product
                cityVM.City = _unitOfWork.City.GetFirstOrDefault(u => u.Id == id);
                return View(cityVM);
            }
      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //protect against anti forgery token attacks.
        public IActionResult Upsert(CityVM obj)
        {
            //if(obj.CategoryName == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("CustomError", "Display Order cannot match the Name");
            //}
            if(ModelState.IsValid)
            {

              
                if(obj.City.Id == 0)
                {
                    _unitOfWork.City.Add(obj.City);
                    TempData["success"] = "City Created Successfully!";
                }
                else
                {
                    _unitOfWork.City.Update(obj.City);
                    TempData["success"] = "City Updated Successfully!";
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
            var objList = _unitOfWork.City.GetAll(includeProperties: "Province");
            return Json(new {data = objList });
        }
        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {

            //var obj = _db.ProductCategories.Find(id);

            var hasFk = _unitOfWork.Suburb.GetAll().Any(x => x.Id == id);

            if (!hasFk)
            {
                var obj = _unitOfWork.City.GetFirstOrDefault(u => u.Id == id);
                if (obj == null)
                {
                    return Json(new { success = false, message = "An error occured while deleting" });
                }
                _unitOfWork.City.Remove(obj);
                _unitOfWork.Save();
                return Json(new { success = true, message = "City Deleted Successfully" });
            }

            return Ok(TempData["error"] = "City cannot be deleted since it has a Suburb associated");


        }
        #endregion

    }

}

