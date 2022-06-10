using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NatuurlikBase.Models
{
    public class ProductInventory
    {
        
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int InventoryItemId { get; set; }
        public InventoryItem? Inventory { get; set; }
        public int InventoryItemQuantity { get; set; }
    }
}
