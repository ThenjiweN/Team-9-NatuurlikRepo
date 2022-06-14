using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IProduceFinishedProduct
    {
        Task ExecuteAsync(Product product, int quantity, string actor);
    }
}