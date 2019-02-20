using Microsoft.EntityFrameworkCore;
using ReFreshMVC.Models;

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
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // DB seed data
        //    //modelBuilder.Entity<___>().HasData();

        //}


        public DbSet<Product> Inventory { get; set; }

    }
}
