﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReFreshMVC.Data;

namespace ReFreshMVC.Migrations
{
    [DbContext(typeof(ReFreshDbContext))]
    partial class ReFreshDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ReFreshMVC.Models.Cart", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Completed");

                    b.Property<int>("Total");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("ReFreshMVC.Models.Order", b =>
                {
                    b.Property<int>("CartID");

                    b.Property<int>("ProductID");

                    b.Property<int>("ExtPrice");

                    b.Property<int>("Qty");

                    b.HasKey("CartID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ReFreshMVC.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<bool>("Meaty");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Price");

                    b.Property<int>("QtyAvail");

                    b.Property<int>("Sku");

                    b.HasKey("ID");

                    b.ToTable("Inventory");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Category = 0,
                            Description = "Delicious home cooked Penne Pasta with red sauce and diced sausage",
                            Image = "penne.jpeg",
                            Meaty = true,
                            Name = "Penne Pasta",
                            Price = 5,
                            QtyAvail = 5,
                            Sku = 1
                        },
                        new
                        {
                            ID = 2,
                            Category = 0,
                            Description = "Take home this take-out. Exquisite pad thai with chicken, no spice",
                            Image = "PadThaiChicken.jpg",
                            Meaty = true,
                            Name = "Pad Thai Chicken",
                            Price = 6,
                            QtyAvail = 5,
                            Sku = 2
                        },
                        new
                        {
                            ID = 3,
                            Category = 3,
                            Description = "Thawed and refrozen, still highly refreshing",
                            Image = "AssortedBerries.jpg",
                            Meaty = false,
                            Name = "Assorted Berries",
                            Price = 3,
                            QtyAvail = 5,
                            Sku = 3
                        },
                        new
                        {
                            ID = 4,
                            Category = 1,
                            Description = "Crispy and cruncy, this sourdough makes the perfect cracker for any soup!",
                            Image = "BreadButt.jpg",
                            Meaty = false,
                            Name = "Sourdough Bread Butt",
                            Price = 1,
                            QtyAvail = 5,
                            Sku = 4
                        },
                        new
                        {
                            ID = 5,
                            Category = 3,
                            Description = "With plenty of squeeze left, this lime will pucker you up.",
                            Image = "Lime.jpg",
                            Meaty = false,
                            Name = "Lime",
                            Price = 1,
                            QtyAvail = 5,
                            Sku = 5
                        },
                        new
                        {
                            ID = 6,
                            Category = 0,
                            Description = "The perfect side to any meal, these beans are refried but truely refreshing",
                            Image = "RefriedBeans.jpg",
                            Meaty = false,
                            Name = "Refried Beans",
                            Price = 2,
                            QtyAvail = 5,
                            Sku = 6
                        },
                        new
                        {
                            ID = 7,
                            Category = 0,
                            Description = "Looking to try that new restraunt, don't just try our Mexican Mystery Bag. Who knows what you will get, but that's part of the fun. Guaranteed to contain Mexican food.",
                            Image = "MysteryMexican.jpg",
                            Meaty = true,
                            Name = "Mexican Mystery Bag",
                            Price = 5,
                            QtyAvail = 5,
                            Sku = 7
                        },
                        new
                        {
                            ID = 8,
                            Category = 3,
                            Description = "Serves One, because who wants to share this amazing appetizer. With just enough for you this Chips and Dip package is perfect for any couch adventure.",
                            Image = "ChipsDip.jpg",
                            Meaty = true,
                            Name = "Chips and Dip",
                            Price = 3,
                            QtyAvail = 5,
                            Sku = 8
                        },
                        new
                        {
                            ID = 9,
                            Category = 2,
                            Description = "Crispy and squishy, this sweet treat will leave you wanting more. Includes chopped nuts and honey filling.",
                            Image = "Baklava.jpg",
                            Meaty = false,
                            Name = "Baklava",
                            Price = 4,
                            QtyAvail = 5,
                            Sku = 9
                        },
                        new
                        {
                            ID = 10,
                            Category = 0,
                            Description = "This sweet breakfast staple contains a half cinnamon raison bagel with creamy almond butter spread on top.",
                            Image = "AlmondButterBagel.jpg",
                            Meaty = false,
                            Name = "Almond Butter Bagel",
                            Price = 4,
                            QtyAvail = 5,
                            Sku = 10
                        });
                });

            modelBuilder.Entity("ReFreshMVC.Models.Order", b =>
                {
                    b.HasOne("ReFreshMVC.Models.Cart", "Cart")
                        .WithMany("Orders")
                        .HasForeignKey("CartID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ReFreshMVC.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
