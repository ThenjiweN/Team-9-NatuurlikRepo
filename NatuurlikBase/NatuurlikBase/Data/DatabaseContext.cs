using Microsoft.EntityFrameworkCore;

namespace NatuurlikBase.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) :
            base(options)
        {

        }
    }
}
