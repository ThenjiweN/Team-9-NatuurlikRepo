
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly DatabaseContext _db;

        public InventoryItemRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<InventoryItem>> GetInventoriesByName(string name)
        {
            return await _db.InventoryItem.Where(x => x.InventoryItemName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();

            //return await this.db.Inventories.Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
            //                                        string.IsNullOrWhiteSpace(name)).ToListAsync();
        }

        public async Task AddInventoryAsync(InventoryItem inventory)
        {
            // To prevent different inventories from having the same name
            if (_db.InventoryItem.Any(x => x.InventoryItemName.ToLower() == inventory.InventoryItemName.ToLower())) return;            

            _db.InventoryItem.Add(inventory);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateInventoryAsync(InventoryItem inventory)
        {
            // To prevent different inventories from having the same name
            //if (db.Inventories.Any(x => x.InventoryId != inventory.InventoryId &&
            //                        x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase))) return;

            if (_db.InventoryItem.Any(x => x.Id != inventory.Id &&
                                    x.InventoryItemName.ToLower() == inventory.InventoryItemName.ToLower())) return;


            var inv = await _db.InventoryItem.FindAsync(inventory.Id);
            if (inv != null)
            {
                inv.InventoryItemName = inventory.InventoryItemName;
                inv.QuantityOnHand = inventory.QuantityOnHand;

                await _db.SaveChangesAsync();
            }
        }

        public async Task<InventoryItem?> GetInventoryItemByIdAsync(int inventoryItemId)
        {
            return await _db.InventoryItem.FindAsync(inventoryItemId);
        }
    }
}