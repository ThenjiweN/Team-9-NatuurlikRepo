using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class WriteOffInventory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [RegularExpression("(^[0-9]*$)", ErrorMessage = "Write-Off Quantity  cannot be negative")]
        [Display(Name ="Write-Off Quantity")]
        public int writeOffQuantity { get; set; }
        public DateTime writeOffDate { get; set; } = DateTime.UtcNow;
        
        public int InventoryItemId { get; set; }
        [ValidateNever]
        [ForeignKey("InventoryItemId")]
        public InventoryItem InventoryItem { get; set; }
        public int writeOffReasonId { get; set; }
        [ValidateNever]
        [ForeignKey("writeOffReasonId")]
        public WriteOffReason WriteOffReason { get; set; }
   
    }
   
   
    
}
