using Microsoft.EntityFrameworkCore.Migrations;

namespace Dropship.Website.Backend.Migrations
{
    public partial class AddMarkdownDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MarkdownDescription",
                table: "Plugins",
                type: "mediumtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "MarkdownDescription",
                table: "Mods",
                type: "mediumtext",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkdownDescription",
                table: "Plugins");

            migrationBuilder.DropColumn(
                name: "MarkdownDescription",
                table: "Mods");
        }
    }
}
