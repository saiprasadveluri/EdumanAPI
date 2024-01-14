using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Initial_DB_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enquiries",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolName = table.Column<string>(nullable: false),
                    ContactName = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    EnqDate = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    OrgName = table.Column<string>(nullable: false),
                    OrgAddress = table.Column<string>(nullable: false),
                    OrgPOC = table.Column<string>(nullable: false),
                    OrgEmail = table.Column<string>(nullable: false),
                    OrgMobile = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrgId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(nullable: false),
                    RoleVal = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    MobNo = table.Column<string>(nullable: false),
                    Emailid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Standards",
                columns: table => new
                {
                    StdId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    StdName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standards", x => x.StdId);
                    table.ForeignKey(
                        name: "FK_Standards_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    SubjectName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubId);
                    table.ForeignKey(
                        name: "FK_Subjects_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOrgMaps",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrgMaps", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_UserOrgMaps_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrgMaps_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrgMaps_UserInfos_UserId",
                        column: x => x.UserId,
                        principalTable: "UserInfos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standards_OrgId_StdName",
                table: "Standards",
                columns: new[] { "OrgId", "StdName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_OrgId_SubjectName",
                table: "Subjects",
                columns: new[] { "OrgId", "SubjectName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserOrgMaps_OrgId",
                table: "UserOrgMaps",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrgMaps_RoleId",
                table: "UserOrgMaps",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrgMaps_UserId_OrgId_RoleId",
                table: "UserOrgMaps",
                columns: new[] { "UserId", "OrgId", "RoleId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enquiries");

            migrationBuilder.DropTable(
                name: "Standards");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "UserOrgMaps");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserInfos");
        }
    }
}
