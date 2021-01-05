using Microsoft.EntityFrameworkCore.Migrations;

namespace Dropship.Website.Backend.Migrations
{
    public partial class AddStarCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StarCount",
                table: "ServerList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarCount",
                table: "Plugins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StarCount",
                table: "Mods",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StarCount",
                table: "ServerList");

            migrationBuilder.DropColumn(
                name: "StarCount",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "StarCount",
                table: "Mods");
        }
    }
}
