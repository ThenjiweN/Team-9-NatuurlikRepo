using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private DatabaseContext _db;

        public SupplierRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Supplier obj)
        {
            _db.Suppliers.Update(obj);

        }

    }
}
