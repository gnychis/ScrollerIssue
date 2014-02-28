using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScrollerIssue.Entities
{
    public class EFDbContext : DbContext
    {
        public DbSet<Unit> Units { get; set; }
        public DbSet<GeoLocation> GeoLocations { get; set; }
    }
}