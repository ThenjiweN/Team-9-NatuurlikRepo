using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class OrderQueryRepository : Repository<OrderQuery>, IOrderQueryRepository
    {
        private DatabaseContext _db;

        public OrderQueryRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderQuery obj)
        {
            _db.OrderQuery.Update(obj);
        }
    }
}
