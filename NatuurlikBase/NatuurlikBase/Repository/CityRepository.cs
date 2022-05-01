
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
    public class CityRepository : Repository<City>, ICityRepository
    {
        private DatabaseContext _db;

        public CityRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(City obj)
        {
            var objFromDb = _db.City.FirstOrDefault(u => u.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.CityName = obj.CityName;
                objFromDb.ProvinceId = obj.ProvinceId;
                
            }
        }
    }
}
