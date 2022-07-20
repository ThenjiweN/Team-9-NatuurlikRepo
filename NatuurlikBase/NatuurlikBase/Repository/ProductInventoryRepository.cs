
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{
    public class ProductInventoryRepository : IProductInventoryRepository
    {
        private readonly DatabaseContext _db;

        public ProductInventoryRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _db.Products.Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
           
            return await _db.Products.Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(x => x.Id == productId);
        }

        public async Task<List<Product>> GetProductsByNameAsync(string productName)
        {
            return await _db.Products.Where(x => (x.Name.ToLower().IndexOf(productName.ToLower()) >= 0 ||
                                                    string.IsNullOrWhiteSpace(productName))).ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var productInv = await _db.Products.FindAsync(product.Id);
            if (productInv != null)
            {
                productInv.Name = product.Name;
                productInv.QuantityOnHand = product.QuantityOnHand;
                productInv.ProductInventories = product.ProductInventories;
                await _db.SaveChangesAsync();
                
            }
        }
    }
}
