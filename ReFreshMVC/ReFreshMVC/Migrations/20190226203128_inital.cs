using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReFreshMVC.Migrations
{
    public partial class inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    Completed = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Sku = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    QtyAvail = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Meaty = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    CartID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Qty = table.Column<int>(nullable: false),
                    ExtPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => new { x.CartID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_Orders_Carts_CartID",
                        column: x => x.CartID,
                        principalTable: "Carts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Inventory_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Inventory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "ID", "Category", "Description", "Image", "Meaty", "Name", "Price", "QtyAvail", "Sku" },
                values: new object[,]
                {
                    { 1, 0, "Delicious home cooked Penne Pasta with red sauce and diced sausage", "penne.jpeg", true, "Penne Pasta", 5, 5, 1 },
                    { 2, 0, "Take home this take-out. Exquisite pad thai with chicken, no spice", "PadThaiChicken.jpg", true, "Pad Thai Chicken", 6, 5, 2 },
                    { 3, 3, "Thawed and refrozen, still highly refreshing", "AssortedBerries.jpg", false, "Assorted Berries", 3, 5, 3 },
                    { 4, 1, "Crispy and cruncy, this sourdough makes the perfect cracker for any soup!", "BreadButt.jpg", false, "Sourdough Bread Butt", 1, 5, 4 },
                    { 5, 3, "With plenty of squeeze left, this lime will pucker you up.", "Lime.jpg", false, "Lime", 1, 5, 5 },
                    { 6, 0, "The perfect side to any meal, these beans are refried but truely refreshing", "RefriedBeans.jpg", false, "Refried Beans", 2, 5, 6 },
                    { 7, 0, "Looking to try that new restraunt, don't just try our Mexican Mystery Bag. Who knows what you will get, but that's part of the fun. Guaranteed to contain Mexican food.", "MysteryMexican.jpg", true, "Mexican Mystery Bag", 5, 5, 7 },
                    { 8, 3, "Serves One, because who wants to share this amazing appetizer. With just enough for you this Chips and Dip package is perfect for any couch adventure.", "ChipsDip.jpg", false, "Chips and Dip", 3, 5, 8 },
                    { 9, 2, "Crispy and squishy, this sweet treat will leave you wanting more. Includes chopped nuts and honey filling.", "Baklava.jpg", false, "Baklava", 4, 5, 9 },
                    { 10, 0, "This sweet breakfast staple contains a half cinnamon raison bagel with creamy almond butter spread on top.", "AlmondButterBagel.jpg", false, "Almond Butter Bagel", 4, 5, 10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductID",
                table: "Orders",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Inventory");
        }
    }
}
