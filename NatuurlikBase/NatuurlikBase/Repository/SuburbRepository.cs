
using NatuurlikBase.Data;
using NatuurlikBase.Models;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{
    public class SuburbRepository : Repository<Suburb>, ISuburbRepository
    {
        private DatabaseContext _db;

        public SuburbRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Suburb obj)
        {
            var objFromDb = _db.Suburb.FirstOrDefault(u => u.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.SuburbName = obj.SuburbName;
                objFromDb.CityId = obj.CityId;
                
            }
        }
    }
}
