
using NatuurlikBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IInventoryItemRepository
    {
        Task<InventoryItem?> GetInventoryItemByIdAsync(int inventoryId);
        Task UpdateInventoryAsync(InventoryItem inventory);
        Task AddInventoryAsync(InventoryItem inventory);
        Task<IEnumerable<InventoryItem>> GetInventoriesByName(string name);

    }
}
