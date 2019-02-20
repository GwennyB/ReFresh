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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DB seed data
            modelBuilder.Entity<Product>().HasData(
                new Product {ID = 1, Sku = 1, Name = "product1", Description = "some Description", Image = "URL", Price = 1 },
                new Product { ID = 2, Sku = 2, Name = "product2", Description = "some Description", Image = "URL", Price = 2 },
                new Product { ID = 3, Sku = 3, Name = "product3", Description = "some Description", Image = "URL", Price = 3 },
                new Product { ID = 4, Sku = 4, Name = "product4", Description = "some Description", Image = "URL", Price = 4 },
                new Product { ID = 5, Sku = 5, Name = "product5", Description = "some Description", Image = "URL", Price = 5 },
                new Product { ID = 6, Sku = 6, Name = "product6", Description = "some Description", Image = "URL", Price = 6 },
                new Product { ID = 7, Sku = 7, Name = "product7", Description = "some Description", Image = "URL", Price = 7 },
                new Product { ID = 8, Sku = 8, Name = "product8", Description = "some Description", Image = "URL", Price = 8 },
                new Product { ID = 9, Sku = 9, Name = "product9", Description = "some Description", Image = "URL", Price = 9 },
                new Product { ID = 10, Sku = 10, Name = "product10", Description = "some Description", Image = "URL", Price = 10 }
            );
        }


        public DbSet<Product> Inventory { get; set; }

    }
}
