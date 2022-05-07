using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IProductBrandRepository : IRepository<ProductBrand>
    {
        void Update(ProductBrand obj);
    }
}
