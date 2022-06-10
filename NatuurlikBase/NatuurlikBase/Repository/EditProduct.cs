
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{
    public class EditProduct : IEditProduct
    {
        private readonly IProductInventoryRepository _productInventoryRepository;

        public EditProduct(IProductInventoryRepository productInventoryRepository)
        {
            _productInventoryRepository = productInventoryRepository;
        }

        public async Task ExecuteAsync(Product product)
        {
            await _productInventoryRepository.UpdateProductAsync(product);
        }
    }
}
