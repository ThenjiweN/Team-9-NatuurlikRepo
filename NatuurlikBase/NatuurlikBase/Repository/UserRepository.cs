
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
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private DatabaseContext _db;

        public UserRepository(DatabaseContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser obj)
        {
            var objFromDb = _db.Users.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.FirstName = obj.FirstName;
                objFromDb.Surname = obj.Surname;
                objFromDb.PhoneNumber = obj.PhoneNumber;
                objFromDb.StreetAddress = obj.StreetAddress;
                objFromDb.CountryId = obj.CountryId;
                objFromDb.ProvinceId = obj.ProvinceId;
                objFromDb.CityId = obj.CityId;
                objFromDb.SuburbId = obj.SuburbId;
                objFromDb.EmailConfirmed = obj.EmailConfirmed;


            }
        }
    }
}
