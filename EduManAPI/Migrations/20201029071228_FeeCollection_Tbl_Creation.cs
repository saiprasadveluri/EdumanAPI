using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class FeeCollection_Tbl_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeCollections_FeeMasters_FeeId",
                table: "FeeCollections");

            migrationBuilder.DropIndex(
                name: "IX_FeeCollections_FeeId_MapId",
                table: "FeeCollections");

            migrationBuilder.DropColumn(
                name: "FeeId",
                table: "FeeCollections");

            migrationBuilder.CreateTable(
                name: "FeeCollectionLineItems",
                columns: table => new
                {
                    LineItemId = table.Column<Guid>(nullable: false),
                    ColId = table.Column<Guid>(nullable: false),
                    FeeId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeCollectionLineItems", x => x.LineItemId);
                    table.ForeignKey(
                        name: "FK_FeeCollectionLineItems_FeeCollections_ColId",
                        column: x => x.ColId,
                        principalTable: "FeeCollections",
                        principalColumn: "FeeColId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeCollectionLineItems_FeeMasters_FeeId",
                        column: x => x.FeeId,
                        principalTable: "FeeMasters",
                        principalColumn: "FeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollectionLineItems_ColId",
                table: "FeeCollectionLineItems",
                column: "ColId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollectionLineItems_FeeId",
                table: "FeeCollectionLineItems",
                column: "FeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeCollectionLineItems");

            migrationBuilder.AddColumn<Guid>(
                name: "FeeId",
                table: "FeeCollections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_FeeId_MapId",
                table: "FeeCollections",
                columns: new[] { "FeeId", "MapId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FeeCollections_FeeMasters_FeeId",
                table: "FeeCollections",
                column: "FeeId",
                principalTable: "FeeMasters",
                principalColumn: "FeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
