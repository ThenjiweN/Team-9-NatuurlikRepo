using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Models;
using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.ViewModels
{
    public class PackageOrderVM
    {
        public PackageOrderProduct PackageOrderProduct { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> OrdersList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProductList { get; set; }
    }
}
