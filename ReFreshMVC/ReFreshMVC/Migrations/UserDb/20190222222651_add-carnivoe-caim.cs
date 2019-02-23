using Microsoft.EntityFrameworkCore.Migrations;

namespace ReFreshMVC.Migrations.UserDb
{
    public partial class addcarnivoecaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EatsMeat",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EatsMeat",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }
    }
}
