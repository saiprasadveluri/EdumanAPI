using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class StuLang_Tbl_Change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentLangMaps_StudentInfos_StudentId",
                table: "studentLangMaps");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "studentLangMaps",
                newName: "StudentStnAcdMapId");

            migrationBuilder.RenameIndex(
                name: "IX_studentLangMaps_StudentId_LanguageId",
                table: "studentLangMaps",
                newName: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_studentLangMaps_StuStdAcdYearMaps_StudentStnAcdMapId",
                table: "studentLangMaps",
                column: "StudentStnAcdMapId",
                principalTable: "StuStdAcdYearMaps",
                principalColumn: "MapId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_studentLangMaps_StuStdAcdYearMaps_StudentStnAcdMapId",
                table: "studentLangMaps");

            migrationBuilder.RenameColumn(
                name: "StudentStnAcdMapId",
                table: "studentLangMaps",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_studentLangMaps_StudentStnAcdMapId_LanguageId",
                table: "studentLangMaps",
                newName: "IX_studentLangMaps_StudentId_LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_studentLangMaps_StudentInfos_StudentId",
                table: "studentLangMaps",
                column: "StudentId",
                principalTable: "StudentInfos",
                principalColumn: "StuId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
