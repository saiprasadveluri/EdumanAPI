using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class DigitalContent_tbl_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DigitalContents",
                columns: table => new
                {
                    DCId = table.Column<Guid>(nullable: false),
                    ChapId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    ContentPath = table.Column<string>(nullable: false),
                    DCType = table.Column<int>(nullable: false),
                    MimeType = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DigitalContents", x => x.DCId);
                    table.ForeignKey(
                        name: "FK_DigitalContents_SubChapeters_ChapId",
                        column: x => x.ChapId,
                        principalTable: "SubChapeters",
                        principalColumn: "ChapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DigitalContents_ChapId",
                table: "DigitalContents",
                column: "ChapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DigitalContents");
        }
    }
}
