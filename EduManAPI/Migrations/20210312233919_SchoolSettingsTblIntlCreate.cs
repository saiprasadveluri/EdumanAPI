using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class SchoolSettingsTblIntlCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolSettings",
                columns: table => new
                {
                    SSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrintLogoFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerLogoFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurAcdYear = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSettings", x => x.SSID);
                    table.ForeignKey(
                        name: "FK_SchoolSettings_AcdYears_CurAcdYear",
                        column: x => x.CurAcdYear,
                        principalTable: "AcdYears",
                        principalColumn: "AcdId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SchoolSettings_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSettings_CurAcdYear",
                table: "SchoolSettings",
                column: "CurAcdYear");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSettings_OrgId",
                table: "SchoolSettings",
                column: "OrgId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolSettings");
        }
    }
}
