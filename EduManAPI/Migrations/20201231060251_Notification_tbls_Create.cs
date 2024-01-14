using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Notification_tbls_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    From = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileGuid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationRecps",
                columns: table => new
                {
                    RID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecpType = table.Column<int>(type: "int", nullable: false),
                    RecpId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationRecps", x => x.RID);
                    table.ForeignKey(
                        name: "FK_NotificationRecps_Notifications_NID",
                        column: x => x.NID,
                        principalTable: "Notifications",
                        principalColumn: "NID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecps_NID",
                table: "NotificationRecps",
                column: "NID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationRecps");

            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
