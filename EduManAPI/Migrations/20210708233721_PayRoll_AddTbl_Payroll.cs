using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class PayRoll_AddTbl_Payroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayRolls",
                columns: table => new
                {
                    PRID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmpID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgAmount = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayRolls", x => x.PRID);
                    table.ForeignKey(
                        name: "FK_PayRolls_Payheads_HID",
                        column: x => x.HID,
                        principalTable: "Payheads",
                        principalColumn: "PHID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayRolls_HID_EmpID",
                table: "PayRolls",
                columns: new[] { "HID", "EmpID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayRolls");
        }
    }
}
