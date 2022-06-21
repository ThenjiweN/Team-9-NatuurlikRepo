using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;

namespace NatuurlikBase.Repository
{
    public class UserCartRepository : Repository<Cart>, IUserCartRepository
    {
        private DatabaseContext _db;

        public UserCartRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public int decreaseCount(Cart userCart, int countToDecrementBy)
        {
            userCart.Count -= countToDecrementBy;
            return userCart.Count;
        }

        public int increaseCount(Cart userCart, int countToIncrementBy)
        {
            userCart.Count += countToIncrementBy;
            return userCart.Count;
        }
    }
}
