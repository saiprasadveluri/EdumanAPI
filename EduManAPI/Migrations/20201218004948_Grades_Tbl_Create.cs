using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Grades_Tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeRanges",
                columns: table => new
                {
                    GRID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinMarks = table.Column<double>(type: "float", nullable: false),
                    MaxMarks = table.Column<double>(type: "float", nullable: false),
                    GradePoint = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeRanges", x => x.GRID);
                    table.ForeignKey(
                        name: "FK_GradeRanges_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeRanges_OrgId",
                table: "GradeRanges",
                column: "OrgId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeRanges");
        }
    }
}
