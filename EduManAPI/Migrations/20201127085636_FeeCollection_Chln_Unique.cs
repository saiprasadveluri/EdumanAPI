using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class FeeCollection_Chln_Unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeCollections_ChlnId",
                table: "FeeCollections");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_ChlnId",
                table: "FeeCollections",
                column: "ChlnId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeCollections_ChlnId",
                table: "FeeCollections");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_ChlnId",
                table: "FeeCollections",
                column: "ChlnId");
        }
    }
}
