using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IEditProduct
    {
        Task ExecuteAsync(Product product);
    }
}