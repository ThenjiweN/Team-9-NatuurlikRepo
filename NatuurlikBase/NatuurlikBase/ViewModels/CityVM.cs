using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Models;

namespace NatuurlikBase.ViewModels
{
    public class CityVM
    {
        public City City { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProvinceList { get; set; }
    }
}
