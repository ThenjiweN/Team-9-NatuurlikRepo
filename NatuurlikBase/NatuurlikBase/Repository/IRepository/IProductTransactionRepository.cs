using NatuurlikBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IProductTransactionRepository
    {
        Task ProduceAsync(Product product, int quantity, string actor);
        Task SellProductAsync(Product product, int quantity, string actor);

        //Add Write Off Product Tasks
        Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFromFilter, DateTime? dateToFilter, ProductTransactionType? transactionType);
    }
}
