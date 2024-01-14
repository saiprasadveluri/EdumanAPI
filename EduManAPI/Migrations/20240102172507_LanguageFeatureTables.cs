using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class LanguageFeatureTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsLanguage",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LangOrdinal",
                table: "Subjects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LanguageMaster",
                columns: table => new
                {
                    LangId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageMaster", x => x.LangId);
                    table.ForeignKey(
                        name: "FK_LanguageMaster_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentLangMap",
                columns: table => new
                {
                    MapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLangMap", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_StudentLangMap_LanguageMaster_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LanguageMaster",
                        principalColumn: "LangId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentLangMap_StudentInfos_StudentId",
                        column: x => x.StudentId,
                        principalTable: "StudentInfos",
                        principalColumn: "StuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageMaster_OrgId",
                table: "LanguageMaster",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLangMap_LanguageId",
                table: "StudentLangMap",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentLangMap_StudentId_LanguageId",
                table: "StudentLangMap",
                columns: new[] { "StudentId", "LanguageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentLangMap");

            migrationBuilder.DropTable(
                name: "LanguageMaster");

            migrationBuilder.DropColumn(
                name: "IsLanguage",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "LangOrdinal",
                table: "Subjects");
        }
    }
}
