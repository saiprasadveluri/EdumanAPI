using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class LangFeature_TblsCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageMaster_Organizations_OrgId",
                table: "LanguageMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentLangMap_LanguageMaster_LanguageId",
                table: "StudentLangMap");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentLangMap_StudentInfos_StudentId",
                table: "StudentLangMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentLangMap",
                table: "StudentLangMap");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageMaster",
                table: "LanguageMaster");

            migrationBuilder.RenameTable(
                name: "StudentLangMap",
                newName: "studentLangMaps");

            migrationBuilder.RenameTable(
                name: "LanguageMaster",
                newName: "LanguageMasters");

            migrationBuilder.RenameIndex(
                name: "IX_StudentLangMap_StudentId_LanguageId",
                table: "studentLangMaps",
                newName: "IX_studentLangMaps_StudentId_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentLangMap_LanguageId",
                table: "studentLangMaps",
                newName: "IX_studentLangMaps_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_LanguageMaster_OrgId",
                table: "LanguageMasters",
                newName: "IX_LanguageMasters_OrgId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_studentLangMaps",
                table: "studentLangMaps",
                column: "MapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageMasters",
                table: "LanguageMasters",
                column: "LangId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageMasters_Organizations_OrgId",
                table: "LanguageMasters",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentLangMaps_LanguageMasters_LanguageId",
                table: "studentLangMaps",
                column: "LanguageId",
                principalTable: "LanguageMasters",
                principalColumn: "LangId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_studentLangMaps_StudentInfos_StudentId",
                table: "studentLangMaps",
                column: "StudentId",
                principalTable: "StudentInfos",
                principalColumn: "StuId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LanguageMasters_Organizations_OrgId",
                table: "LanguageMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_studentLangMaps_LanguageMasters_LanguageId",
                table: "studentLangMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_studentLangMaps_StudentInfos_StudentId",
                table: "studentLangMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_studentLangMaps",
                table: "studentLangMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LanguageMasters",
                table: "LanguageMasters");

            migrationBuilder.RenameTable(
                name: "studentLangMaps",
                newName: "StudentLangMap");

            migrationBuilder.RenameTable(
                name: "LanguageMasters",
                newName: "LanguageMaster");

            migrationBuilder.RenameIndex(
                name: "IX_studentLangMaps_StudentId_LanguageId",
                table: "StudentLangMap",
                newName: "IX_StudentLangMap_StudentId_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_studentLangMaps_LanguageId",
                table: "StudentLangMap",
                newName: "IX_StudentLangMap_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_LanguageMasters_OrgId",
                table: "LanguageMaster",
                newName: "IX_LanguageMaster_OrgId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentLangMap",
                table: "StudentLangMap",
                column: "MapId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LanguageMaster",
                table: "LanguageMaster",
                column: "LangId");

            migrationBuilder.AddForeignKey(
                name: "FK_LanguageMaster_Organizations_OrgId",
                table: "LanguageMaster",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLangMap_LanguageMaster_LanguageId",
                table: "StudentLangMap",
                column: "LanguageId",
                principalTable: "LanguageMaster",
                principalColumn: "LangId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentLangMap_StudentInfos_StudentId",
                table: "StudentLangMap",
                column: "StudentId",
                principalTable: "StudentInfos",
                principalColumn: "StuId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
