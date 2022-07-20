using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Models;

namespace NatuurlikBase.ViewModels
{
    public class UserCartVM
    {

        public IList<Cart> CartList { get; set; }
        public Order Order { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ProvinceList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SuburbList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CourierList { get; set; }
    }
}
