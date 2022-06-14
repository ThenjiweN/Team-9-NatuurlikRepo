using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IValidateEnoughInventories
    {
        Task<bool> ExecuteAsync(Product product, int quantity);
    }
}