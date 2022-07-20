using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface ICourierRepository : IRepository<Courier>
    {
        void Update(Courier obj);
    }
}
