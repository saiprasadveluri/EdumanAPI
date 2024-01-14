using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class FeeCollection_ChlnCol_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChlnId",
                table: "FeeCollections",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_ChlnId",
                table: "FeeCollections",
                column: "ChlnId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeCollections_Chalans_ChlnId",
                table: "FeeCollections",
                column: "ChlnId",
                principalTable: "Chalans",
                principalColumn: "ChlId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeCollections_Chalans_ChlnId",
                table: "FeeCollections");

            migrationBuilder.DropIndex(
                name: "IX_FeeCollections_ChlnId",
                table: "FeeCollections");

            migrationBuilder.DropColumn(
                name: "ChlnId",
                table: "FeeCollections");
        }
    }
}
