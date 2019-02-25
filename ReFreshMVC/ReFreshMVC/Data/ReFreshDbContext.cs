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

            modelBuilder.Entity<Product>().HasData(
                new Product { ID = 1, Sku = 1, Name = "Penne Pasta", Description = "Delicious home cooked Penne Pasta with red sauce and diced sausage", Image = "~/Img/penne.jpeg", Price = 5, Category = Categories.entrees, Meaty = true },
                new Product { ID = 2, Sku = 2, Name = "Pad Thai Chicken", Description = "Take home this take-out. Exquisite pad thai with chicken, no spice", Image = "~/Img/PadThaiChicken.jpeg", Price = 6, Category = Categories.entrees, Meaty = true },
                new Product { ID = 3, Sku = 3, Name = "Assorted Berries", Description = "Thawed and refrozen, still highly refreshing", Image = "~/Img/AssortedBerries.jpeg", Price = 3, Category = Categories.snacks, Meaty = false },
                new Product { ID = 4, Sku = 4, Name = "Sourdough Bread Butt", Description = "Crispy and cruncy, this sourdough makes the perfect cracker for any soup!", Image = "~/Img/BreadButt.jpg", Price = 1, Category = Categories.sides, Meaty = false },
                new Product { ID = 5, Sku = 5, Name = "Lime", Description = "With plenty of squeeze left, this lime will pucker you up.", Image = "~/Img/Lime.jpeg", Price = 1, Category = Categories.snacks, Meaty = false },
                new Product { ID = 6, Sku = 6, Name = "Refried Beans", Description = "The perfect side to any meal, these beans are refried but truely refreshing", Image = "~/Img/RefriedBeans.jpg", Price = 2, Category = Categories.entrees, Meaty = false },
                new Product { ID = 7, Sku = 7, Name = "Mexican Mystery Bag", Description = "Looking to try that new restraunt, don't just try our Mexican Mystery Bag. Who knows what you will get, but that's part of the fun. Guaranteed to contain Mexican food.", Image = "~/Img/MysteryMexican.jpg", Price = 5, Category = Categories.entrees, Meaty = true },
                new Product { ID = 8, Sku = 8, Name = "Chips and Dip", Description = "Serves One, because who wants to share this amazing appetizer. With just enough for you this Chips and Dip package is perfect for any couch adventure.", Image = "~/Img/ChipsDip", Price = 3, Category = Categories.snacks, Meaty = false },
                new Product { ID = 9, Sku = 9, Name = "Baklava", Description = "Crispy and squishy, this sweet treat will leave you wanting more. Includes chopped nuts and honey filling.", Image = "~/Img/Baklava.jpg", Price = 4, Category = Categories.desserts, Meaty = false },
                new Product { ID = 10, Sku = 10, Name = "Almond Butter Bagel", Description = "This sweet breakfast staple contains a whole wheat biscuit with creamy peanut butter spread on top.", Image = "~/Img/AlmondButterBagel", Price = 4, Category = Categories.entrees, Meaty = false }
            );
        }
        
        public DbSet<Product> Inventory { get; set; }
    }
}

