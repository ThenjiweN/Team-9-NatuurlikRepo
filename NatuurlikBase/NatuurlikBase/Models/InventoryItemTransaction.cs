using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Models
{
    public class InventoryItemTransaction
    {
        [Key]
        public int InventoryItemTransactionId { get; set; }        

        [Required]
        public int InventoryItemId { get; set; }

        [Required]
        [Display(Name ="Quantity Before")]
        public int QuantityBefore { get; set; }

        // This is the action taken (purchase or product product)
        [Required]
        public InventoryItemTransactionType ActivityType { get; set; }

        [Required]
        [Display(Name = "Quantity After")]
        public int QuantityAfter { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string Actor { get; set; }

        public InventoryItem InventoryItem { get; set; }

    }
}
