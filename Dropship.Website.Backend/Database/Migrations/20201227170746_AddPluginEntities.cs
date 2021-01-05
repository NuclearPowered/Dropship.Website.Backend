using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dropship.Website.Backend.Migrations
{
    public partial class AddPluginEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plugins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "mediumtext", nullable: false),
                    CreatorUserId = table.Column<int>(type: "int", nullable: false),
                    ServerDistroId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plugins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plugins_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PluginBuilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PluginId = table.Column<int>(type: "int", nullable: false),
                    VersionCode = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "varchar(16) CHARACTER SET utf8mb4", maxLength: 16, nullable: false),
                    FileName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    DownloadUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginBuilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PluginBuilds_Plugins_PluginId",
                        column: x => x.PluginId,
                        principalTable: "Plugins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PluginBuilds_PluginId_Version",
                table: "PluginBuilds",
                columns: new[] { "PluginId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_PluginBuilds_PluginId_VersionCode",
                table: "PluginBuilds",
                columns: new[] { "PluginId", "VersionCode" });

            migrationBuilder.CreateIndex(
                name: "IX_Plugins_CreatorUserId",
                table: "Plugins",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Plugins_Guid",
                table: "Plugins",
                column: "Guid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PluginBuilds");

            migrationBuilder.DropTable(
                name: "Plugins");
        }
    }
}
