using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IViewProductsByName
    {
        Task<List<Product>> ExecuteSearchAsync(string name = "");
        Task<List<Product>> ViewAll();
    }
}