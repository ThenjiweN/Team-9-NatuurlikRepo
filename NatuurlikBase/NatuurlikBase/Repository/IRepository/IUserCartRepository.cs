
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IUserCartRepository : IRepository<Cart>
    {
        // Manage cart methods to be implemented
        int decreaseCount(Cart userCart, int countToDecrementBy);
        int increaseCount(Cart userCart, int countToIncrementBy);
    }
}
