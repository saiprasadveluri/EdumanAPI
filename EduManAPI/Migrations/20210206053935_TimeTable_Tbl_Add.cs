using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class TimeTable_Tbl_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeTables",
                columns: table => new
                {
                    TTID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekNo = table.Column<int>(type: "int", nullable: false),
                    HourNo = table.Column<int>(type: "int", nullable: false),
                    StnSubMapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTables", x => x.TTID);
                    table.ForeignKey(
                        name: "FK_TimeTables_StdSubMaps_StnSubMapId",
                        column: x => x.StnSubMapId,
                        principalTable: "StdSubMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeTables_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_StnSubMapId",
                table: "TimeTables",
                column: "StnSubMapId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_TeacherId",
                table: "TimeTables",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeTables");
        }
    }
}
