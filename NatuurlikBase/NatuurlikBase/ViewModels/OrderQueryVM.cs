using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatuurlikBase.Models;
using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.ViewModels
{
    public class OrderQueryVM
    {
        public OrderQuery OrderQuery { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> OrdersList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> QueryReasons { get; set; }
    }
}
