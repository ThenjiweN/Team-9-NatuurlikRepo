
using NatuurlikBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository.IRepository
{
    public interface ISuburbRepository : IRepository<Suburb>
    {
        void Update(Suburb obj);

    }
}
