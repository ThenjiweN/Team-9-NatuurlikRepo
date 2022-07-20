using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);


    }
}
