
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class ViewInventoriesByName : IViewInventoriesByName
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public ViewInventoriesByName(IInventoryItemRepository inventoryitemRepository)
        {
            _inventoryItemRepository = inventoryitemRepository;
        }

        public async Task<IEnumerable<InventoryItem>> ExecuteAsync(string name = "")
        {
            return await _inventoryItemRepository.GetInventoriesByName(name);
        }
    }
}