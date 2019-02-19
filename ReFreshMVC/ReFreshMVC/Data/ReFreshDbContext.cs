using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Data
{
    public class ReFreshDbContext : DbContext
    {

        public ReFreshDbContext(DbContextOptions<ReFreshDbContext> options) : base(options)
        {

        }


        /// <summary>
        /// overrides (DbContext virtual) method that builds out basic API structure
        /// maps composite keys
        /// </summary>
        /// <param name="modelBuilder">  </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DB seed data
            //modelBuilder.Entity<___>().HasData();

        }


        //public DbSet<___> Posts { get; set; }

    }
}
}
