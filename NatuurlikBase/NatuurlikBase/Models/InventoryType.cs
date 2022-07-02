using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class InventoryType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Inventory Type Name")]
        [MaxLength(50)]
        public string InventoryTypeName { get; set; }
    }
}
