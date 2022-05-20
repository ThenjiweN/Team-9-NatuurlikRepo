using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class Courier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Courier Name")]
        [MaxLength(25)]
        public string CourierName { get; set; }

        [Required(ErrorMessage = "Courier Fee is required")]
        [Display(Name = "Courier Fee")]
        [Range(1, 10000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CourierFee { get; set; }

        [Required]
        [Display(Name = "Estimated Delivery Time")]
        public string EstimatedDeliveryTime { get; set; }
    }
}
