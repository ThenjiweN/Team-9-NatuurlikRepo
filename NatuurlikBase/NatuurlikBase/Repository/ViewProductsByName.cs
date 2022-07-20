
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
    public class ViewProductsByName : IViewProductsByName
    {
        private readonly IProductInventoryRepository _productInventoryRepository;
        private readonly DatabaseContext _db;

        public ViewProductsByName(IProductInventoryRepository productInventoryRepository, DatabaseContext db)
        {
            _productInventoryRepository = productInventoryRepository;
            _db = db;
        }

        public async Task<List<Product>> ExecuteSearchAsync(string name = "")
        {
            return await _productInventoryRepository.GetProductsByNameAsync(name);
        }

        public async Task<List<Product>> ViewAll()
        {
            return await _productInventoryRepository.GetAllProductsAsync();
        }
    }
}
