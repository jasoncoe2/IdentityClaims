using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityClaims.Models
{
    public class IdentityClaimsContext : DbContext
    {
        public IdentityClaimsContext(DbContextOptions<IdentityClaimsContext> options) : base (options)
        {

        }

        public DbSet<Lookup> Lookups { get; set; }

        public DbSet<LookupValue> LookupValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lookup>().ToTable("Lookup");
            modelBuilder.Entity<LookupValue>().ToTable("LookupValue");
        }
    }
}
