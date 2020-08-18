using System;
using Microsoft.EntityFrameworkCore;

namespace SafeAuto.Models
{
    public class SafeAutoContext : DbContext
    {
        public SafeAutoContext(DbContextOptions<SafeAutoContext> options)
            : base (options)
        {
        }

        public DbSet<Trip> Trip { get; set; }
    }
}
