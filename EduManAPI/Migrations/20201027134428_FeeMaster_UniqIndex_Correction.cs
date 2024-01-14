using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class FeeMaster_UniqIndex_Correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeMasters_FHeadId_TermNo",
                table: "FeeMasters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FeeMasters_FHeadId_TermNo",
                table: "FeeMasters",
                columns: new[] { "FHeadId", "TermNo" },
                unique: true);
        }
    }
}
