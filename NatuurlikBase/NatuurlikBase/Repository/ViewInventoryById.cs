using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{
    public class ViewInventoryById : IViewInventoryById
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public ViewInventoryById(IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<InventoryItem?> ExecuteAsync(int inventoryItemId)
        {
            return await _inventoryItemRepository.GetInventoryItemByIdAsync(inventoryItemId);
        }
    }
}
