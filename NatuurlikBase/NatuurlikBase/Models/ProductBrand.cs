using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class ProductBrand
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Product Brand")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
