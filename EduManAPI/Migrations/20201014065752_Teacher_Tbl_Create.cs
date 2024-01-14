using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Teacher_Tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    EmpId = table.Column<string>(nullable: false),
                    FName = table.Column<string>(nullable: false),
                    MName = table.Column<string>(nullable: true),
                    LName = table.Column<string>(nullable: true),
                    DOJoining = table.Column<DateTime>(type: "date", nullable: false),
                    Address = table.Column<string>(nullable: true),
                    MobileNo = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(nullable: true),
                    MotherName = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    BllodGroup = table.Column<string>(nullable: true),
                    TeacherType = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    LoginUID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherId);
                    table.ForeignKey(
                        name: "FK_Teachers_UserInfos_LoginUID",
                        column: x => x.LoginUID,
                        principalTable: "UserInfos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teachers_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_LoginUID",
                table: "Teachers",
                column: "LoginUID");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_OrgId_EmpId",
                table: "Teachers",
                columns: new[] { "OrgId", "EmpId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
