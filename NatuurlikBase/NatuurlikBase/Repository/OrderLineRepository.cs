using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class OrderLineRepository : Repository<OrderLine>, IOrderLineRepository
    {
        private DatabaseContext _db;

        public OrderLineRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderLine obj)
        {
            _db.OrderLine.Update(obj);
        }
    }
}
