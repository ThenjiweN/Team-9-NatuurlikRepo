using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class OrderLine
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }

        //Retrieve all products that have been added to the cart at checkout.
        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public decimal Price { get; set; }
        public int Count { get; set; }

    }
}
