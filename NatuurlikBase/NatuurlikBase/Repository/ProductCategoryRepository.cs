using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private DatabaseContext _db;

        public ProductCategoryRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(ProductCategory obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
