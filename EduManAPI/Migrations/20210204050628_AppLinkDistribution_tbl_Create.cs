using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class AppLinkDistribution_tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLinkDistributions",
                columns: table => new
                {
                    ALID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StuMapId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLinkDistributions", x => x.ALID);
                    table.ForeignKey(
                        name: "FK_AppLinkDistributions_StuStdAcdYearMaps_StuMapId",
                        column: x => x.StuMapId,
                        principalTable: "StuStdAcdYearMaps",
                        principalColumn: "MapId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppLinkDistributions_StuMapId",
                table: "AppLinkDistributions",
                column: "StuMapId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLinkDistributions");
        }
    }
}
