
using NatuurlikBase.Data;
using NatuurlikBase.Repository;
using NatuurlikBase.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private DatabaseContext _db;

        public UnitOfWork(DatabaseContext db)
        {
            _db = db;
            Country = new CountryRepository(_db);
            Province = new ProvinceRepository(_db);
            City = new CityRepository(_db);
            Suburb = new SuburbRepository(_db);
            User = new UserRepository(_db);
          
        }
        public ICountryRepository Country { get; private set; }
        public IProvinceRepository Province { get; private set; }
        public ICityRepository City { get; private set; }
        public ISuburbRepository Suburb { get; private set; }
        public IUserRepository User { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
