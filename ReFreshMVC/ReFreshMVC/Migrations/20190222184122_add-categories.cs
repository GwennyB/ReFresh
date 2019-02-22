using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReFreshMVC.Migrations
{
    public partial class addcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Sku = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Inventory",
                columns: new[] { "ID", "Category", "Description", "Image", "Name", "Price", "Sku" },
                values: new object[,]
                {
                    { 1, 0, "Delicious home cooked Penne Pasta with red sauce and diced sausage", "https://via.placeholder.com/150", "Penne Pasta", 5, 1 },
                    { 2, 0, "Take home this take-out. Exquisite pad thai with chicken, no spice", "https://via.placeholder.com/150", "Pad Thai Chicken", 6, 2 },
                    { 3, 0, "Thawed and refrozen, still highly refreshing", "https://via.placeholder.com/150", "Assorted Berries", 3, 3 },
                    { 4, 0, "Crispy and cruncy, this sourdough makes the perfect cracker for any soup!", "https://via.placeholder.com/150", "Sourdough Bread Butt", 1, 4 },
                    { 5, 0, "With plenty of squeeze left, this lime will pucker you up.", "https://via.placeholder.com/150", "Lime", 1, 5 },
                    { 6, 0, "The perfect side to any meal, these beans are refried but truely refreshing", "https://via.placeholder.com/150", "Refried Beans", 2, 6 },
                    { 7, 0, "Looking to try that new restraunt, don't just try our Mexican Mystery Bag. Who knows what you will get, but that's part of the fun. Guaranteed to contain Mexican food.", "https://via.placeholder.com/150", "Mexican Mystery Bag", 5, 7 },
                    { 8, 0, "Serves One, because who wants to share this amazing appetizer. With just enough for you this Chips and Dip package is perfect for any couch adventure.", "https://via.placeholder.com/150", "Chips and Dip", 3, 8 },
                    { 9, 0, "Crispy and squishy, this sweet treat will leave you wanting more. Includes chopped nuts and honey filling.", "https://via.placeholder.com/150", "Baklava", 4, 9 },
                    { 10, 0, "This sweet breakfast staple contains a whole wheat biscuit with creamy peanut butter spread on top.", "https://via.placeholder.com/150", "Peanut Butter Bisquit", 4, 10 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");
        }
    }
}
