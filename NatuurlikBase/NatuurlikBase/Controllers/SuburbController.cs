using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Repository.IRepository;
using NatuurlikBase.ViewModels;

namespace NatuurlikBase.Controllers
{
    public class SuburbController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public SuburbController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            SuburbVM suburbVM = new()
            {
                Suburb = new(),
                CityList = _unitOfWork.City.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CityName,
                    Value = i.Id.ToString()
                })
            };

        

            if (id== null || id == 0)
            {
                //Create the product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["BrandsList"] = BrandsList;
                return View(suburbVM);
            }
            else
            {
                //update the product
                suburbVM.Suburb = _unitOfWork.Suburb.GetFirstOrDefault(u => u.Id == id);
                return View(suburbVM);
            }
      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //protect against anti forgery token attacks.
        public IActionResult Upsert(SuburbVM obj)
        {
            //if(obj.CategoryName == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("CustomError", "Display Order cannot match the Name");
            //}
            if(ModelState.IsValid)
            {

              
                if(obj.Suburb.Id == 0)
                {
                    _unitOfWork.Suburb.Add(obj.Suburb);
                    TempData["success"] = "Suburb Created Successfully!";
                }
                else
                {
                    _unitOfWork.Suburb.Update(obj.Suburb);
                    TempData["success"] = "Suburb Updated Successfully!";
                }
                
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(obj);
       
        }


        

        ////GET
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //var categoryFromDb = _db.ProductCategories.Find(id);
        //    var objFromDbFirst = _unitOfWork.Brand.GetFirstOrDefault(u => u.Id == id);
        //    if (objFromDbFirst == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(objFromDbFirst);
        //}

        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var objList = _unitOfWork.Suburb.GetAll(includeProperties: "City");
            return Json(new {data = objList });
        }
        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {

            //var obj = _db.ProductCategories.Find(id);

            var obj = _unitOfWork.Suburb.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "An error occured while deleting" });
            }
            _unitOfWork.Suburb.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Suburb Deleted Successfully" });


        }
        #endregion

    }

}

