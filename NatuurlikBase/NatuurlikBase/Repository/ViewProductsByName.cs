
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

        public ViewProductsByName(IProductInventoryRepository productInventoryRepository)
        {
            _productInventoryRepository = productInventoryRepository;
        }

        public async Task<List<Product>> ExecuteSearchAsync(string name = "")
        {
            return await _productInventoryRepository.GetProductsByNameAsync(name);
        }
    }
}
