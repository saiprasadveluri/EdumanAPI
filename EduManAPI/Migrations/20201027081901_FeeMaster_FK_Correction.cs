using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class FeeMaster_FK_Correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeMasters_StuStdAcdYearMaps_AcdyearId",
                table: "FeeMasters");

            migrationBuilder.DropIndex(
                name: "IX_FeeCollections_FeeId",
                table: "FeeCollections");

            migrationBuilder.CreateIndex(
                name: "IX_FeeMasters_FHeadId_TermNo",
                table: "FeeMasters",
                columns: new[] { "FHeadId", "TermNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_FeeId_MapId",
                table: "FeeCollections",
                columns: new[] { "FeeId", "MapId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeMasters_AcdYears_AcdyearId",
                table: "FeeMasters",
                column: "AcdyearId",
                principalTable: "AcdYears",
                principalColumn: "AcdId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeMasters_AcdYears_AcdyearId",
                table: "FeeMasters");

            migrationBuilder.DropIndex(
                name: "IX_FeeMasters_FHeadId_TermNo",
                table: "FeeMasters");

            migrationBuilder.DropIndex(
                name: "IX_FeeCollections_FeeId_MapId",
                table: "FeeCollections");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_FeeId",
                table: "FeeCollections",
                column: "FeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeMasters_StuStdAcdYearMaps_AcdyearId",
                table: "FeeMasters",
                column: "AcdyearId",
                principalTable: "StuStdAcdYearMaps",
                principalColumn: "MapId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
