using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICountryRepository Country { get; }
        IProvinceRepository Province { get; }
        ICityRepository City { get; }
        ISuburbRepository Suburb { get; }
        IUserRepository User { get; }
        IProductRepository Product { get; }
        IProductBrandRepository Brand { get; }
        IProductCategoryRepository Category { get; }
        ISupplierRepository Supplier { get; }

        void Save();
    }
}
