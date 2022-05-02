using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Customer Price")]
        public double CustomerPrice { get; set; }

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Reseller Price")]
        public double ResellerPrice { get; set; }


        [ValidateNever]
        public string PictureUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public ProductCategory Category { get; set; }

        [Required]
        [Display(Name = "Product Brand")]
        public int ProductBrandId { get; set; }
        [ValidateNever]
        public ProductBrand Brand { get; set; }
    }
}
