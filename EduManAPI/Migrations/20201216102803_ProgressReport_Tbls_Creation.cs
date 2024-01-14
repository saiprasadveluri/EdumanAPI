using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class ProgressReport_Tbls_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamProgressReportHeads",
                columns: table => new
                {
                    PRHId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeadName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxMarks = table.Column<double>(type: "float", nullable: false),
                    MinMarks = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamProgressReportHeads", x => x.PRHId);
                    table.ForeignKey(
                        name: "FK_ExamProgressReportHeads_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamProgressReports",
                columns: table => new
                {
                    PRId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StuMapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PRHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Marks = table.Column<double>(type: "float", nullable: false),
                    SubwiseRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamProgressReports", x => x.PRId);
                    table.ForeignKey(
                        name: "FK_ExamProgressReports_ExamProgressReportHeads_PRHeadId",
                        column: x => x.PRHeadId,
                        principalTable: "ExamProgressReportHeads",
                        principalColumn: "PRHId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ExamProgressReports_ExamSchedules_SchId",
                        column: x => x.SchId,
                        principalTable: "ExamSchedules",
                        principalColumn: "ExamSchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamProgressReports_StuStdAcdYearMaps_StuMapId",
                        column: x => x.StuMapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamProgressReportHeads_ExamId",
                table: "ExamProgressReportHeads",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamProgressReports_PRHeadId",
                table: "ExamProgressReports",
                column: "PRHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamProgressReports_SchId",
                table: "ExamProgressReports",
                column: "SchId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamProgressReports_StuMapId",
                table: "ExamProgressReports",
                column: "StuMapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamProgressReports");

            migrationBuilder.DropTable(
                name: "ExamProgressReportHeads");
        }
    }
}
