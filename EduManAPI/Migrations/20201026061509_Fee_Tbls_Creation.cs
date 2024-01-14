using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Fee_Tbls_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeHeadMasters",
                columns: table => new
                {
                    FeeHeadId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    FeeHeadName = table.Column<string>(nullable: false),
                    FeeType = table.Column<int>(nullable: false),
                    Terms = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeHeadMasters", x => x.FeeHeadId);
                    table.ForeignKey(
                        name: "FK_FeeHeadMasters_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeMasters",
                columns: table => new
                {
                    FeeId = table.Column<Guid>(nullable: false),
                    FHeadId = table.Column<Guid>(nullable: false),
                    TermNo = table.Column<int>(nullable: false),
                    StnId = table.Column<Guid>(nullable: true),
                    MapId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    AcdyearId = table.Column<Guid>(nullable: false),
                    DueDayNo = table.Column<int>(nullable: false),
                    DueMonthNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeMasters", x => x.FeeId);
                    table.ForeignKey(
                        name: "FK_FeeMasters_StuStdAcdYearMaps_AcdyearId",
                        column: x => x.AcdyearId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeMasters_StuStdAcdYearMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeMasters_Standards_StnId",
                        column: x => x.StnId,
                        principalTable: "Standards",
                        principalColumn: "StdId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeCollections",
                columns: table => new
                {
                    FeeColId = table.Column<Guid>(nullable: false),
                    FeeId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    ColDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeCollections", x => x.FeeColId);
                    table.ForeignKey(
                        name: "FK_FeeCollections_FeeMasters_FeeId",
                        column: x => x.FeeId,
                        principalTable: "FeeMasters",
                        principalColumn: "FeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeCollections_StuStdAcdYearMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_FeeId",
                table: "FeeCollections",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCollections_MapId",
                table: "FeeCollections",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeHeadMasters_OrgId",
                table: "FeeHeadMasters",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeMasters_AcdyearId",
                table: "FeeMasters",
                column: "AcdyearId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeMasters_MapId",
                table: "FeeMasters",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeMasters_StnId",
                table: "FeeMasters",
                column: "StnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeCollections");

            migrationBuilder.DropTable(
                name: "FeeHeadMasters");

            migrationBuilder.DropTable(
                name: "FeeMasters");
        }
    }
}
