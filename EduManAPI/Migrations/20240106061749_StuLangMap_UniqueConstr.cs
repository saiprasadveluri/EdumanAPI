using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class StuLangMap_UniqueConstr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId",
                table: "studentLangMaps");

            migrationBuilder.CreateIndex(
                name: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId_LangOrdinal",
                table: "studentLangMaps",
                columns: new[] { "StudentStnAcdMapId", "LanguageId", "LangOrdinal" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId_LangOrdinal",
                table: "studentLangMaps");

            migrationBuilder.CreateIndex(
                name: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId",
                table: "studentLangMaps",
                columns: new[] { "StudentStnAcdMapId", "LanguageId" },
                unique: true);
        }
    }
}
