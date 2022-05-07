
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace NatuurlikBase.Controllers
{
    public class DropDownListController : Controller
    {

        private readonly DatabaseContext _db;
        public DropDownListController(DatabaseContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> countryNames = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            LocationVM locVM = new LocationVM();

            List<Country> countries = _db.Country.OrderBy(x => x.CountryName).ToList();
            countries.ForEach(x =>
            {
                countryNames.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = x.CountryName, Value = x.Id.ToString() });
            });
            locVM.CountryNames = countryNames;
            return View(locVM);
        }
        
        //Get all provinces by selected countryId
        // return name and Id to populate select list item
        [HttpGet]
        public ActionResult GetProvince (int countryId)
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
    }
}
