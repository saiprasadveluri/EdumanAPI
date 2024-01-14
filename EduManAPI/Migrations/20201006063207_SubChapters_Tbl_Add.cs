using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class SubChapters_Tbl_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubChapeters",
                columns: table => new
                {
                    ChapId = table.Column<Guid>(nullable: false),
                    MapId = table.Column<Guid>(nullable: false),
                    ChapName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubChapeters", x => x.ChapId);
                    table.ForeignKey(
                        name: "FK_SubChapeters_StdSubMaps_MapId",
                        column: x => x.MapId,
                        principalTable: "StdSubMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubChapeters_MapId_ChapName",
                table: "SubChapeters",
                columns: new[] { "MapId", "ChapName" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubChapeters");
        }
    }
}
