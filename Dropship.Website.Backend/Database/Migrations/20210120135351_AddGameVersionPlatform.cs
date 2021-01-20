using Microsoft.EntityFrameworkCore.Migrations;

namespace Dropship.Website.Backend.Migrations
{
    public partial class AddGameVersionPlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GamePlatform",
                table: "ModBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GameVersion",
                table: "ModBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamePlatform",
                table: "ModBuilds");

            migrationBuilder.DropColumn(
                name: "GameVersion",
                table: "ModBuilds");
        }
    }
}
