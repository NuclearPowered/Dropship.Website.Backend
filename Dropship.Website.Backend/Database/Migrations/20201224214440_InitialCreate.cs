using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dropship.Website.Backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FriendlyName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Xml = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(16) CHARACTER SET utf8mb4", maxLength: 16, nullable: false),
                    Email = table.Column<string>(type: "varchar(320) CHARACTER SET utf8mb4", maxLength: 320, nullable: false),
                    Password = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "mediumtext", nullable: false),
                    CreatorUserId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mods_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "varchar(320) CHARACTER SET utf8mb4", maxLength: 320, nullable: false),
                    IpAddress = table.Column<int>(type: "int(16)", nullable: false),
                    Port = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    OwnerUserId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerList_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModBuilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModId = table.Column<int>(type: "int", nullable: false),
                    VersionCode = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "varchar(16) CHARACTER SET utf8mb4", maxLength: 16, nullable: false),
                    FileName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    DownloadUrl = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModBuilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModBuilds_Mods_ModId",
                        column: x => x.ModId,
                        principalTable: "Mods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModDeps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ModBuildId = table.Column<int>(type: "int", nullable: false),
                    DepModBuildId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModDeps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModDeps_ModBuilds_DepModBuildId",
                        column: x => x.DepModBuildId,
                        principalTable: "ModBuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModDeps_ModBuilds_ModBuildId",
                        column: x => x.ModBuildId,
                        principalTable: "ModBuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModBuilds_ModId_Version",
                table: "ModBuilds",
                columns: new[] { "ModId", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_ModBuilds_ModId_VersionCode",
                table: "ModBuilds",
                columns: new[] { "ModId", "VersionCode" });

            migrationBuilder.CreateIndex(
                name: "IX_ModDeps_DepModBuildId",
                table: "ModDeps",
                column: "DepModBuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDeps_ModBuildId",
                table: "ModDeps",
                column: "ModBuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_CreatorUserId",
                table: "Mods",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mods_Guid",
                table: "Mods",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerList_IpAddress_Port",
                table: "ServerList",
                columns: new[] { "IpAddress", "Port" });

            migrationBuilder.CreateIndex(
                name: "IX_ServerList_OwnerUserId",
                table: "ServerList",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "ModDeps");

            migrationBuilder.DropTable(
                name: "ServerList");

            migrationBuilder.DropTable(
                name: "ModBuilds");

            migrationBuilder.DropTable(
                name: "Mods");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
