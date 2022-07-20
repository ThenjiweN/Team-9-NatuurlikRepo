using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class QueryReasonRepository : Repository<QueryReason>, IQueryReasonRepository
    {
        private DatabaseContext _db;

        public QueryReasonRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(QueryReason obj)
        {
            _db.QueryReason.Update(obj);

        }

    }
}
