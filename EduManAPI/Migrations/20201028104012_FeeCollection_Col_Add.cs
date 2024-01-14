using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class FeeCollection_Col_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FeeCollections",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayType",
                table: "FeeCollections",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FeeCollections");

            migrationBuilder.DropColumn(
                name: "PayType",
                table: "FeeCollections");
        }
    }
}
