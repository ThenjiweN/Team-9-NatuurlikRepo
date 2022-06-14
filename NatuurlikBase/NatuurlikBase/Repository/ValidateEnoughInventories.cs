
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{    
    public class ValidateEnoughInventories : IValidateEnoughInventories
    {
        //Add product repo
        private readonly IProductInventoryRepository _productRepository;

        public ValidateEnoughInventories(IProductInventoryRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> ExecuteAsync(Product product, int quantity)
        {
            var productToProduce = await _productRepository.GetProductByIdAsync(product.Id);
            foreach (var inv in productToProduce.ProductInventories)
            {
                if (inv.InventoryItemQuantity * quantity > inv.Inventory.QuantityOnHand)
                    return false;
            }

            return true;
        }
    }
}
