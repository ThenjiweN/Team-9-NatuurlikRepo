

using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IViewInventoryById
    {
        Task<InventoryItem?> ExecuteAsync(int inventoryId);
    }
}