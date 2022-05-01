using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatuurlikBase.Models;

namespace NatuurlikBase.Data;

public class DatabaseContext : IdentityDbContext<ApplicationUser>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Country { get; set; }
    public DbSet<Province> Province { get; set; }
    public DbSet<City> City { get; set; }
    public DbSet<Suburb> Suburb { get; set; }
    public DbSet<ApplicationUser> User { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }

    
}

