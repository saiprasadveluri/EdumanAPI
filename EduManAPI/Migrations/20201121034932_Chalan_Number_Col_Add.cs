using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Chalan_Number_Col_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChlnNumber",
                table: "Chalans",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SNo",
                table: "Chalans",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chalans_ChlnNumber",
                table: "Chalans",
                column: "ChlnNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chalans_ChlnNumber",
                table: "Chalans");

            migrationBuilder.DropColumn(
                name: "ChlnNumber",
                table: "Chalans");

            migrationBuilder.DropColumn(
                name: "SNo",
                table: "Chalans");
        }
    }
}
