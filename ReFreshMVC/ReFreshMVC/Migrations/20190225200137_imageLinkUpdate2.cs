using Microsoft.EntityFrameworkCore.Migrations;

namespace ReFreshMVC.Migrations
{
    public partial class imageLinkUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 2,
                column: "Image",
                value: "PadThaiChicken.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 3,
                column: "Image",
                value: "AssortedBerries.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 5,
                column: "Image",
                value: "Lime.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Image",
                value: "ChipsDip.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 10,
                column: "Image",
                value: "AlmondButterBagel.jpg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 2,
                column: "Image",
                value: "PadThaiChicken.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 3,
                column: "Image",
                value: "AssortedBerries.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 5,
                column: "Image",
                value: "Lime.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Image",
                value: "ChipsDip");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 10,
                column: "Image",
                value: "AlmondButterBagel");
        }
    }
}
