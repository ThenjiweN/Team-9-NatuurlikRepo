using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {
        void Update(ProductCategory obj);
    }
}
