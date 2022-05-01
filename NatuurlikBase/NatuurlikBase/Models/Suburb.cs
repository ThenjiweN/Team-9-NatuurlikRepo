using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class Suburb
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Suburb Name")]
        public string SuburbName { get; set; }
        [Required]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^(\d{4})$", ErrorMessage = "Only numerical values are allowed. Please re-enter.")]
        [MaxLength(4)]
        public string PostalCode { get; set; }
        public int CityId { get; set; }
        [ValidateNever]
        [ForeignKey("CityId")]
        public City City { get; set; }
    }
}
