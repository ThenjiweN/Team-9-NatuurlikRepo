using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IOrderLineRepository : IRepository<OrderLine>
    {
        void Update(OrderLine obj);
    }
}
