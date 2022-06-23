using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class SupplierInventory
    {
        
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public int InventoryItemId { get; set; }
        public InventoryItem? Inventory { get; set; }
        public int SupplierOrderQuantity { get; set; }
    }
}
