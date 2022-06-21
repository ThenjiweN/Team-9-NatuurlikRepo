using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private readonly DatabaseContext _db;
        private readonly IProductInventoryRepository _productInventoryRepository;

        public ProductTransactionRepository(DatabaseContext db, IProductInventoryRepository productInventoryRepository)
        {
            _productInventoryRepository = productInventoryRepository;
            _db = db;
        }

        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(
            string productName,
            DateTime? dateFromFilter,
            DateTime? dateToFilter,
            ProductTransactionType? transactionType
            
            )
            
        {
            if (dateToFilter.HasValue) dateToFilter = dateToFilter.Value.AddDays(1);
            var query = from p in _db.ProductTransaction
                        join prod in _db.Products on p.ProductId equals prod.Id
                        where 
                            (string.IsNullOrWhiteSpace(productName) || prod.Name.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                            (!dateFromFilter.HasValue || p.TransactionDate >= dateFromFilter.Value.Date) &&
                            (!dateToFilter.HasValue || p.TransactionDate <= dateToFilter.Value.Date) &&
                            (!transactionType.HasValue || p.ActivityType == transactionType)
                        select p;

            return await query.Include(x => x.Product).ToListAsync();
        }

        public async Task ProduceAsync(Product product, int productionQuantity, string actor)
        {
       
            var prod = await _productInventoryRepository.GetProductByIdAsync(product.Id);     
            

            if (prod != null)
            {
                foreach(var pi in prod.ProductInventories)
                {
                    int qtyBefore = pi.Inventory.QuantityOnHand;
                    pi.Inventory.QuantityOnHand -= productionQuantity * pi.InventoryItemQuantity;                  

                    _db.InventoryItemTransaction.Add(new InventoryItemTransaction
                    {
                        InventoryItemId = pi.InventoryItemId,
                        QuantityBefore = qtyBefore,
                        ActivityType = InventoryItemTransactionType.ProduceProduct,
                        QuantityAfter = pi.Inventory.QuantityOnHand,
                        TransactionDate = DateTime.Now,
                        Actor = actor
                        
                       
                    });
                }
            }            

            _db.ProductTransaction.Add(new ProductTransaction
            {
                ProductId = product.Id,
                QuantityBefore = product.QuantityOnHand,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.QuantityOnHand + productionQuantity,
                TransactionDate = DateTime.Now,
                Actor = actor
            });
            await _db.SaveChangesAsync();
        }

        public async Task SellProductAsync(Product product, int qty, string actor)
        {
            _db.ProductTransaction.Add(new ProductTransaction
            {
                ProductId = product.Id,
                ActivityType = ProductTransactionType.SellProduct,
                QuantityBefore = product.QuantityOnHand,
                QuantityAfter = product.QuantityOnHand - qty,
                TransactionDate = DateTime.Now,
                Actor = actor
                
            });
            await _db.SaveChangesAsync();
        }
    }
}
