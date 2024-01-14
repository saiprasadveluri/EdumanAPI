using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class StdSubMap_Table_Creation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StdSubMaps",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    StdId = table.Column<Guid>(nullable: true),
                    SubId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StdSubMaps", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_StdSubMaps_Standards_StdId",
                        column: x => x.StdId,
                        principalTable: "Standards",
                        principalColumn: "StdId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StdSubMaps_Subjects_SubId",
                        column: x => x.SubId,
                        principalTable: "Subjects",
                        principalColumn: "SubId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StdSubMaps_SubId",
                table: "StdSubMaps",
                column: "SubId");

            migrationBuilder.CreateIndex(
                name: "IX_StdSubMaps_StdId_SubId",
                table: "StdSubMaps",
                columns: new[] { "StdId", "SubId" },
                unique: true,
                filter: "[StdId] IS NOT NULL AND [SubId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StdSubMaps");
        }
    }
}
