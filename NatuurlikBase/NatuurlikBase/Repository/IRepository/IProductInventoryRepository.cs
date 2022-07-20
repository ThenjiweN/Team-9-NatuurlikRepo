
using NatuurlikBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IProductInventoryRepository
    {
        Task<List<Product>> GetProductsByNameAsync(string name);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task UpdateProductAsync(Product product);

       
    }
}
