

using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository

{
    public interface IViewProductById
    {
        Task<Product> ExecuteAsync(int productId);
    }
}