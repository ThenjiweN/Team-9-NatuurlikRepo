using NatuurlikBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IInventoryItemTransactionRepository
    {
        Task ProcureAsync(InventoryItem inventory, int quantity, string doneBy);
        Task<IEnumerable<InventoryItemTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFromFilter, DateTime? dateToFilter, InventoryItemTransactionType? inventoryTransactionType);
    }
}
