using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class OnlineSessionAttendee_tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnlineSessionAttendees",
                columns: table => new
                {
                    AtId = table.Column<Guid>(nullable: false),
                    OSId = table.Column<Guid>(nullable: false),
                    EvtDate = table.Column<DateTime>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineSessionAttendees", x => x.AtId);
                    table.ForeignKey(
                        name: "FK_OnlineSessionAttendees_StuStdAcdYearMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OnlineSessionAttendees_OnLineSessionInfos_OSId",
                        column: x => x.OSId,
                        principalTable: "OnLineSessionInfos",
                        principalColumn: "OSId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineSessionAttendees_MapId",
                table: "OnlineSessionAttendees",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_OnlineSessionAttendees_OSId",
                table: "OnlineSessionAttendees",
                column: "OSId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnlineSessionAttendees");
        }
    }
}
