
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{
    public class InventoryItemTransactionRepository : IInventoryItemTransactionRepository
    {
        private readonly DatabaseContext _db;

        public InventoryItemTransactionRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<InventoryItemTransaction>> GetInventoryTransactionsAsync(
            string inventoryName, 
            DateTime? dateFrom, 
            DateTime? dateTo, 
            InventoryItemTransactionType? transactionType)
        {
            if (dateTo.HasValue) dateTo = dateTo.Value.AddDays(1);

            var query = from invT in _db.InventoryItemTransaction
                        join inv in _db.InventoryItem on invT.InventoryItemId equals inv.Id
                        where 
                            (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryItemName.ToLower().IndexOf(inventoryName.ToLower()) >= 0) &&
                            (!dateFrom.HasValue || invT.TransactionDate >= dateFrom.Value.Date) &&
                            (!dateTo.HasValue || invT.TransactionDate <= dateTo.Value.Date) &&
                            (!transactionType.HasValue || invT.ActivityType == transactionType)
                        select invT;

            return await query.Include(x => x.InventoryItem).ToListAsync();
        }        

        public async Task ProcureAsync(InventoryItem inventory, int quantity, string actor)
        {
            _db.InventoryItemTransaction.Add(new InventoryItemTransaction
            {
                InventoryItemId = inventory.Id,
                QuantityBefore = inventory.QuantityOnHand,
                ActivityType = InventoryItemTransactionType.ProcureInventory,
                QuantityAfter = inventory.QuantityOnHand + quantity,
                TransactionDate = DateTime.Now,
                Actor = actor,
            });
            await _db.SaveChangesAsync();
        }
    }
}
