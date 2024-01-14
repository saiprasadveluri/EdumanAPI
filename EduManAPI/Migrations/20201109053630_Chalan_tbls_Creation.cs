using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Chalan_tbls_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chalans",
                columns: table => new
                {
                    ChlId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    ChlDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chalans", x => x.ChlId);
                    table.ForeignKey(
                        name: "FK_Chalans_StuStdAcdYearMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChalanLineInfos",
                columns: table => new
                {
                    ChlLineId = table.Column<Guid>(nullable: false),
                    ChlId = table.Column<Guid>(nullable: false),
                    TermNo = table.Column<int>(nullable: false),
                    FeeId = table.Column<Guid>(nullable: false),
                    FeeHeadName = table.Column<string>(nullable: false),
                    TotAmt = table.Column<double>(nullable: false),
                    PaidAmt = table.Column<double>(nullable: false),
                    DueMon = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChalanLineInfos", x => x.ChlLineId);
                    table.ForeignKey(
                        name: "FK_ChalanLineInfos_Chalans_ChlId",
                        column: x => x.ChlId,
                        principalTable: "Chalans",
                        principalColumn: "ChlId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChalanLineInfos_FeeMasters_FeeId",
                        column: x => x.FeeId,
                        principalTable: "FeeMasters",
                        principalColumn: "FeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChalanLineInfos_ChlId",
                table: "ChalanLineInfos",
                column: "ChlId");

            migrationBuilder.CreateIndex(
                name: "IX_ChalanLineInfos_FeeId",
                table: "ChalanLineInfos",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Chalans_MapId",
                table: "Chalans",
                column: "MapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChalanLineInfos");

            migrationBuilder.DropTable(
                name: "Chalans");
        }
    }
}
