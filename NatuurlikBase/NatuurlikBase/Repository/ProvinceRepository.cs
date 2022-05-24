
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
    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        private DatabaseContext _db;

        public ProvinceRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Province obj)
        {
            var objFromDb = _db.Province.FirstOrDefault(u => u.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.ProvinceName = obj.ProvinceName;
                objFromDb.CountryId = obj.CountryId;
                
            }
        }

        //public IList<Province> getProvincesByCountry(int countryId)
        //{
        //    var provinceList = _db.Province.Where(x => countryId == x.CountryId).ToList();
        //    return provinceList;
        //}
    }
}
