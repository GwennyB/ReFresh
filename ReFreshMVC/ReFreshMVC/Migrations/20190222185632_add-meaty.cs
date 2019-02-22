using Microsoft.EntityFrameworkCore.Migrations;

namespace ReFreshMVC.Migrations
{
    public partial class addmeaty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Meaty",
                table: "Inventory",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 1,
                column: "Meaty",
                value: true);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 2,
                column: "Meaty",
                value: true);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 3,
                column: "Category",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 4,
                column: "Category",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 5,
                column: "Category",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 7,
                column: "Meaty",
                value: true);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Category",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 9,
                column: "Category",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 10,
                column: "Name",
                value: "Almond Butter Bagel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Meaty",
                table: "Inventory");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 3,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 4,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 5,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 9,
                column: "Category",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 10,
                column: "Name",
                value: "Peanut Butter Bisquit");
        }
    }
}
