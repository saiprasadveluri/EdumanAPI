using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Student_InfoTbl_Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcdYears",
                columns: table => new
                {
                    AcdId = table.Column<Guid>(nullable: false),
                    AcdText = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcdYears", x => x.AcdId);
                });

            migrationBuilder.CreateTable(
                name: "StudentInfos",
                columns: table => new
                {
                    StuId = table.Column<Guid>(nullable: false),
                    RegdNo = table.Column<string>(nullable: false),
                    FName = table.Column<string>(nullable: false),
                    MName = table.Column<string>(nullable: true),
                    LName = table.Column<string>(nullable: true),
                    DOBirth = table.Column<DateTime>(type: "date", nullable: false),
                    DOAdmission = table.Column<DateTime>(type: "date", nullable: false),
                    ResAddress = table.Column<string>(nullable: true),
                    FatherName = table.Column<string>(nullable: true),
                    MotherName = table.Column<string>(nullable: true),
                    FatherMobile = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    BloodGroup = table.Column<string>(nullable: true),
                    ParentEmail = table.Column<string>(nullable: true),
                    AadharNo = table.Column<string>(nullable: true),
                    Religion = table.Column<string>(nullable: true),
                    Cast = table.Column<string>(nullable: true),
                    SchoolAdmNo = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false, defaultValue: 1),
                    LoginUID = table.Column<Guid>(nullable: true),
                    StuImageFile = table.Column<Guid>(nullable: true),
                    StuFatherImageFile = table.Column<Guid>(nullable: true),
                    StuMotherImageFile = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInfos", x => x.StuId);
                    table.ForeignKey(
                        name: "FK_StudentInfos_UserInfos_LoginUID",
                        column: x => x.LoginUID,
                        principalTable: "UserInfos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StuStdAcdYearMaps",
                columns: table => new
                {
                    MapId = table.Column<Guid>(nullable: false),
                    AcYearId = table.Column<Guid>(nullable: false),
                    StnId = table.Column<Guid>(nullable: false),
                    StuId = table.Column<Guid>(nullable: false),
                    RecType = table.Column<int>(nullable: false, defaultValue: 0),
                    RecDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StuStdAcdYearMaps", x => x.MapId);
                    table.ForeignKey(
                        name: "FK_StuStdAcdYearMaps_AcdYears_AcYearId",
                        column: x => x.AcYearId,
                        principalTable: "AcdYears",
                        principalColumn: "AcdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StuStdAcdYearMaps_Standards_StnId",
                        column: x => x.StnId,
                        principalTable: "Standards",
                        principalColumn: "StdId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StuStdAcdYearMaps_StudentInfos_StuId",
                        column: x => x.StuId,
                        principalTable: "StudentInfos",
                        principalColumn: "StuId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentInfos_LoginUID",
                table: "StudentInfos",
                column: "LoginUID");

            migrationBuilder.CreateIndex(
                name: "IX_StuStdAcdYearMaps_StnId",
                table: "StuStdAcdYearMaps",
                column: "StnId");

            migrationBuilder.CreateIndex(
                name: "IX_StuStdAcdYearMaps_StuId",
                table: "StuStdAcdYearMaps",
                column: "StuId");

            migrationBuilder.CreateIndex(
                name: "IX_StuStdAcdYearMaps_AcYearId_StnId_StuId",
                table: "StuStdAcdYearMaps",
                columns: new[] { "AcYearId", "StnId", "StuId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StuStdAcdYearMaps");

            migrationBuilder.DropTable(
                name: "AcdYears");

            migrationBuilder.DropTable(
                name: "StudentInfos");
        }
    }
}
