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

        [Display(Name = "Quantity On Hand")]
        public int QuantityOnHand { get; set; } = 0;

        [Range(1, 10000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CustomerPrice { get; set; }


        [Range(1, 10000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ResellerPrice { get; set; }


        [ValidateNever]
        [Display(Name = "Product Image")]
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

        [Display(Name = "Display Product")]
        public string DisplayProduct { get; set; }

        public List<ProductInventory>? ProductInventories { get; set; }

        [Display(Name = "Threshold Value")]
        public int ThresholdValue { get; set; }
    }
}
