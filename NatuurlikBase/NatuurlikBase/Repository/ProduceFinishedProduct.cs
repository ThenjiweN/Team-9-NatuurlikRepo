using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{
    public class ProduceFinishedProduct : IProduceFinishedProduct
    {
        private readonly IProductInventoryRepository _productRepository;
        private readonly IProductTransactionRepository _productTransactionRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IInventoryItemTransactionRepository _inventoryItemTransactionRepository;


        public ProduceFinishedProduct(
            IInventoryItemRepository inventoryRepository,
            IProductInventoryRepository productRepository,
            IInventoryItemTransactionRepository inventoryTransactionRepository,
            IProductTransactionRepository productTransactionRepository)
        {
            _productRepository = productRepository;
            _productTransactionRepository = productTransactionRepository;
            _inventoryItemTransactionRepository = inventoryTransactionRepository;
            _inventoryItemRepository = inventoryRepository;
        }

        public async Task ExecuteAsync(Product product, int quantity, string actor)
        {
            await _productTransactionRepository.ProduceAsync(product, quantity, actor);

            product.QuantityOnHand += quantity;
            await _productRepository.UpdateProductAsync(product);
        }

    }
}
