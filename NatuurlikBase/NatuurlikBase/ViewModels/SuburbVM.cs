using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Models;

namespace NatuurlikBase.ViewModels
{
    public class SuburbVM
    {
        public Suburb Suburb { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
