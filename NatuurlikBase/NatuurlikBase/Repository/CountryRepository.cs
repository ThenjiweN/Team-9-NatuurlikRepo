
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
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private DatabaseContext _db;

        public CountryRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Country obj)
        {
            _db.Country.Update(obj);
        }
    }
}
