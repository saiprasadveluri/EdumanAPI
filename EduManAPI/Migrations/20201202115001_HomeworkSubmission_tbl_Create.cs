using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class HomeworkSubmission_tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeWorkSubmissions",
                columns: table => new
                {
                    HWSubId = table.Column<Guid>(nullable: false),
                    HWId = table.Column<Guid>(nullable: false),
                    StuMapId = table.Column<Guid>(nullable: false),
                    SubDate = table.Column<DateTime>(type: "date", nullable: false),
                    HWSubPath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeWorkSubmissions", x => x.HWSubId);
                    table.ForeignKey(
                        name: "FK_HomeWorkSubmissions_HomeWorks_HWId",
                        column: x => x.HWId,
                        principalTable: "HomeWorks",
                        principalColumn: "HWId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomeWorkSubmissions_StuStdAcdYearMaps_StuMapId",
                        column: x => x.StuMapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorkSubmissions_HWId",
                table: "HomeWorkSubmissions",
                column: "HWId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeWorkSubmissions_StuMapId",
                table: "HomeWorkSubmissions",
                column: "StuMapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeWorkSubmissions");
        }
    }
}
