using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class CoCurricularMark_tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoCurricularMarks",
                columns: table => new
                {
                    CCID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StuMapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValueEducation = table.Column<double>(type: "float", nullable: false),
                    ComputerEducation = table.Column<double>(type: "float", nullable: false),
                    CulturalEducation = table.Column<double>(type: "float", nullable: false),
                    PhysicalEducation = table.Column<double>(type: "float", nullable: false),
                    OtherAreas = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoCurricularMarks", x => x.CCID);
                    table.ForeignKey(
                        name: "FK_CoCurricularMarks_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoCurricularMarks_StuStdAcdYearMaps_StuMapId",
                        column: x => x.StuMapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoCurricularMarks_ExamId",
                table: "CoCurricularMarks",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_CoCurricularMarks_StuMapId",
                table: "CoCurricularMarks",
                column: "StuMapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoCurricularMarks");
        }
    }
}
