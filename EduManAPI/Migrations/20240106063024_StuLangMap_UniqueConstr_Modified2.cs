using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class StuLangMap_UniqueConstr_Modified2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId",
                table: "studentLangMaps",
                columns: new[] { "StudentStnAcdMapId", "LanguageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId",
                table: "studentLangMaps");
        }
    }
}
