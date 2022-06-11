using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class InventoryProcured
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }

        [RegularExpression("(^[0-9]*$)", ErrorMessage = "This value cannot be negative")]
        public int QuantityReceived { get; set; }
        public DateTime DateLogged { get; set; } = DateTime.Now;

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        [ValidateNever]
        public Supplier Supplier { get; set; }

        public int ItemID { get; set; }
        [ForeignKey("ItemID")]
        [ValidateNever]
        public InventoryItem InventoryItem { get; set; }

        [ValidateNever]
        [Display(Name = "Supplier Invoice File")]
        public string? InvoiceFile { get; set; }
    }
}

