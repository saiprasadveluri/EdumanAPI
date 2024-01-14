using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class OnLineSession_tbls_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineSessionUrls",
                columns: table => new
                {
                    OSUrlId = table.Column<Guid>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    SessionUrl = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineSessionUrls", x => x.OSUrlId);
                    table.ForeignKey(
                        name: "FK_OnlineSessionUrls_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "TeacherId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnLineSessionInfos",
                columns: table => new
                {
                    OSId = table.Column<Guid>(nullable: false),
                    SubMapId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsReapated = table.Column<int>(nullable: false),
                    RepeatString = table.Column<string>(nullable: true),
                    OSessionUrlId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnLineSessionInfos", x => x.OSId);
                    table.ForeignKey(
                        name: "FK_OnLineSessionInfos_OnlineSessionUrls_OSessionUrlId",
                        column: x => x.OSessionUrlId,
                        principalTable: "OnlineSessionUrls",
                        principalColumn: "OSUrlId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnLineSessionInfos_StdSubMaps_SubMapId",
                        column: x => x.SubMapId,
                        principalTable: "StdSubMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnLineSessionInfos_OSessionUrlId",
                table: "OnLineSessionInfos",
                column: "OSessionUrlId");

            migrationBuilder.CreateIndex(
                name: "IX_OnLineSessionInfos_SubMapId",
                table: "OnLineSessionInfos",
                column: "SubMapId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineSessionUrls_TeacherId",
                table: "OnlineSessionUrls",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnLineSessionInfos");

            migrationBuilder.DropTable(
                name: "OnlineSessionUrls");
        }
    }
}
