using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Models;

namespace NatuurlikBase.ViewModels
{
    public class UserVM
    {
        public ApplicationUser User { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProvinceForCountryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CityForProvinceList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SuburbForCityList { get; set; }
    }
}
