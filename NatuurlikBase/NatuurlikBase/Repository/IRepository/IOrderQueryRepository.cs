using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IOrderQueryRepository : IRepository<OrderQuery>
    {
        void Update(OrderQuery obj);
    }
}
