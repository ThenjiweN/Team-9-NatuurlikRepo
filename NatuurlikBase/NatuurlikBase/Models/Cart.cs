using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class Cart
    {
        public Product Product { get; set; }
        [Range(1, 10000, ErrorMessage = "At least one item is needed")]
        public int Count { get; set; }
    }
}
