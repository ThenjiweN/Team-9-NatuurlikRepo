using NatuurlikBase.Models;

namespace NatuurlikBase.Repository.IRepository
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        void Update(Supplier obj);
    }
}
