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
        Task<IEnumerable<ProductTransaction>> ProductTransactions(string productName, ProductTransactionType? transactionType, DateTime? dateFromFilter, DateTime? dateToFilter);

        Task ProduceAsync(Product product, int quantity, string actor);
        Task PackageOrderAsync(Product product, int productQuantity, string actor);

    }
}
