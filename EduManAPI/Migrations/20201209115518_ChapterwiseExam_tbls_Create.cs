using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class ChapterwiseExam_tbls_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatpterwiseExams",
                columns: table => new
                {
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatpterwiseExams", x => x.ExamId);
                });

            migrationBuilder.CreateTable(
                name: "ChapterwiseExamChapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChapterwiseExamChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChapterwiseExamChapters_ChatpterwiseExams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "ChatpterwiseExams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChapterwiseExamChapters_SubChapeters_ChapId",
                        column: x => x.ChapId,
                        principalTable: "SubChapeters",
                        principalColumn: "ChapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChapterwiseExamChapters_ChapId",
                table: "ChapterwiseExamChapters",
                column: "ChapId");

            migrationBuilder.CreateIndex(
                name: "IX_ChapterwiseExamChapters_ExamId",
                table: "ChapterwiseExamChapters",
                column: "ExamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChapterwiseExamChapters");

            migrationBuilder.DropTable(
                name: "ChatpterwiseExams");
        }
    }
}
