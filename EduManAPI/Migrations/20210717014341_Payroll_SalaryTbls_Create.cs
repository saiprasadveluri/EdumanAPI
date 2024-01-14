using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Payroll_SalaryTbls_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salaries",
                columns: table => new
                {
                    SALID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MonthNo = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.SALID);
                });

            migrationBuilder.CreateTable(
                name: "SalaryDetails",
                columns: table => new
                {
                    SALDetID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeadName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    HeadType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryDetails", x => x.SALDetID);
                    table.ForeignKey(
                        name: "FK_SalaryDetails_Salaries_SalID",
                        column: x => x.SalID,
                        principalTable: "Salaries",
                        principalColumn: "SALID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_EmpId_MonthNo_Year",
                table: "Salaries",
                columns: new[] { "EmpId", "MonthNo", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryDetails_SalID",
                table: "SalaryDetails",
                column: "SalID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryDetails");

            migrationBuilder.DropTable(
                name: "Salaries");
        }
    }
}
