using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class TimeTableSettings_tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeTableSettings",
                columns: table => new
                {
                    TTSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeTableSettings", x => x.TTSID);
                    table.ForeignKey(
                        name: "FK_TimeTableSettings_Standards_StnId",
                        column: x => x.StnId,
                        principalTable: "Standards",
                        principalColumn: "StdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTableSettings_StnId",
                table: "TimeTableSettings",
                column: "StnId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeTableSettings");
        }
    }
}
