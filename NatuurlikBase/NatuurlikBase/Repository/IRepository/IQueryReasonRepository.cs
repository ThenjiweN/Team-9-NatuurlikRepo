using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IQueryReasonRepository : IRepository<QueryReason>
    {
        void Update(QueryReason obj);
    }
}
