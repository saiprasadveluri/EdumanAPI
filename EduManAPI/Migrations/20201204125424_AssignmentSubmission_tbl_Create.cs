using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class AssignmentSubmission_tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignmentSubmissions",
                columns: table => new
                {
                    AssnSubId = table.Column<Guid>(nullable: false),
                    AssnId = table.Column<Guid>(nullable: false),
                    StuMapId = table.Column<Guid>(nullable: false),
                    SubDate = table.Column<DateTime>(type: "date", nullable: false),
                    AssnSubPath = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentSubmissions", x => x.AssnSubId);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_Assignments_AssnId",
                        column: x => x.AssnId,
                        principalTable: "Assignments",
                        principalColumn: "AssnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentSubmissions_StuStdAcdYearMaps_StuMapId",
                        column: x => x.StuMapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_AssnId",
                table: "AssignmentSubmissions",
                column: "AssnId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentSubmissions_StuMapId",
                table: "AssignmentSubmissions",
                column: "StuMapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentSubmissions");
        }
    }
}
