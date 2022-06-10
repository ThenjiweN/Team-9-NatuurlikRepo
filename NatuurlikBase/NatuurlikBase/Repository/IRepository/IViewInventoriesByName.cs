using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IViewInventoriesByName
    {
        Task<IEnumerable<InventoryItem>> ExecuteAsync(string name = "");
    }
}