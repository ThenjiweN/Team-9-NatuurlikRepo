using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Models;

namespace NatuurlikBase.ViewModels
{
    public class ProvinceVM
    {
        public Province Province { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}
