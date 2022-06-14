using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.ViewModels
{
    public class ProduceVM
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "At least 1 item has to be produced.")]
        public int QuantityToProduce { get; set; }

    }
}
