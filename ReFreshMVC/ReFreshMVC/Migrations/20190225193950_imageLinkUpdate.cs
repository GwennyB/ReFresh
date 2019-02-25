using Microsoft.EntityFrameworkCore.Migrations;

namespace ReFreshMVC.Migrations
{
    public partial class imageLinkUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 1,
                column: "Image",
                value: "penne.jpeg");

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
                keyValue: 4,
                column: "Image",
                value: "BreadButt.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 5,
                column: "Image",
                value: "Lime.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 6,
                column: "Image",
                value: "RefriedBeans.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 7,
                column: "Image",
                value: "MysteryMexican.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Image",
                value: "ChipsDip");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 9,
                column: "Image",
                value: "Baklava.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 10,
                column: "Image",
                value: "AlmondButterBagel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 1,
                column: "Image",
                value: "~/Img/penne.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 2,
                column: "Image",
                value: "~/Img/PadThaiChicken.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 3,
                column: "Image",
                value: "~/Img/AssortedBerries.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 4,
                column: "Image",
                value: "~/Img/BreadButt.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 5,
                column: "Image",
                value: "~/Img/Lime.jpeg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 6,
                column: "Image",
                value: "~/Img/RefriedBeans.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 7,
                column: "Image",
                value: "~/Img/MysteryMexican.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Image",
                value: "~/Img/ChipsDip");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 9,
                column: "Image",
                value: "~/Img/Baklava.jpg");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 10,
                column: "Image",
                value: "~/Img/AlmondButterBagel");
        }
    }
}
