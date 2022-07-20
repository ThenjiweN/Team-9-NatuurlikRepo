using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class CourierRepository : Repository<Courier>, ICourierRepository
    {
        private DatabaseContext _db;

        public CourierRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Courier obj)
        {
            _db.Courier.Update(obj);
        }
    }
}
