using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class PackageOrderProduct
    {
        public int Id { get; set; }

        [Required]
      
        [Display(Name = "Product Quantity")]
        public int ProductQuantity { get; set; } = 0;

        [Required]
        [Display(Name = "Order Number")]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        public string? ActorName { get; set; }

    }
}
