using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class Cart
    {
        public int Id { get; set; }
        //add foreign key of Nav prop for product
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        //Do not validate the navigation properties
        [ValidateNever]
        public Product Product { get; set; }

        [Range(1, 10000, ErrorMessage = "At least one item is needed")]
        public int Count { get; set; }

        [NotMapped]
        public decimal CartItemPrice { get; set; }

        //Add User details
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

    }
}
