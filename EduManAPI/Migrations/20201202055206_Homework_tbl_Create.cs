using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Homework_tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeWorks",
                columns: table => new
                {
                    HWId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    HWDate = table.Column<DateTime>(type: "date", nullable: false),
                    HomeWorkString = table.Column<string>(nullable: true),
                    ClassWorkString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeWorks", x => x.HWId);
                    table.ForeignKey(
                        name: "FK_HomeWorks_StdSubMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StdSubMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorks_MapId",
                table: "HomeWorks",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeWorks");
        }
    }
}
