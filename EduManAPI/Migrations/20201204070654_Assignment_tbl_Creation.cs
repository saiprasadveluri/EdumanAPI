using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Assignment_tbl_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    AssnId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    AssnDate = table.Column<DateTime>(type: "date", nullable: false),
                    Title = table.Column<string>(nullable: false),
                    AssnFileName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.AssnId);
                    table.ForeignKey(
                        name: "FK_Assignments_StdSubMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StdSubMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_MapId",
                table: "Assignments",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");
        }
    }
}
