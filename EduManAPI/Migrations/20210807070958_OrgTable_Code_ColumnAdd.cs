using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class OrgTable_Code_ColumnAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrgCode",
                table: "Organizations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrgCode",
                table: "Organizations",
                column: "OrgCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrgCode",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrgCode",
                table: "Organizations");
        }
    }
}
