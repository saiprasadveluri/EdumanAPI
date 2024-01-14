using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class FeeConcession_tbl_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "FeeCollectionLineItems",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FeeConcessions",
                columns: table => new
                {
                    ConId = table.Column<Guid>(nullable: false),
                    FeeId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Reason = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeConcessions", x => x.ConId);
                    table.ForeignKey(
                        name: "FK_FeeConcessions_StuStdAcdYearMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeConcessions_MapId",
                table: "FeeConcessions",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeConcessions_FeeId_MapId",
                table: "FeeConcessions",
                columns: new[] { "FeeId", "MapId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeConcessions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "FeeCollectionLineItems");
        }
    }
}
