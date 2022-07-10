using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private DatabaseContext _db;

        public ProductRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.CustomerPrice = obj.CustomerPrice;
                objFromDb.ResellerPrice = obj.ResellerPrice;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.ProductBrandId = obj.ProductBrandId;
                objFromDb.QuantityOnHand = obj.QuantityOnHand;
                objFromDb.DisplayProduct = obj.DisplayProduct;
                objFromDb.ThresholdValue = obj.ThresholdValue;
                if (obj.PictureUrl != null)
                {
                    objFromDb.PictureUrl = obj.PictureUrl;
                }
            }
        }
    }
}
