using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "City Name")]
        [MaxLength(25)]
        public string CityName { get; set; }
        public int ProvinceId { get; set; }
        [ValidateNever]
        [ForeignKey("ProvinceId")]
        public Province Province { get; set; }
    }
}
