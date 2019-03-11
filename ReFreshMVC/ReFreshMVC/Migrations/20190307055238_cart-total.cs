using Microsoft.EntityFrameworkCore.Migrations;

namespace ReFreshMVC.Migrations
{
    public partial class carttotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "Carts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Meaty",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Carts");

            migrationBuilder.UpdateData(
                table: "Inventory",
                keyColumn: "ID",
                keyValue: 8,
                column: "Meaty",
                value: false);
        }
    }
}
