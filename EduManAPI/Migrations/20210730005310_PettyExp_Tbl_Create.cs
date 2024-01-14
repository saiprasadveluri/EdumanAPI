using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class PettyExp_Tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "DiscountVal",
                table: "Bills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "PettyExpnses",
                columns: table => new
                {
                    PEID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EHeadID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    AccountTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChequeOrRefNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileGuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PettyExpnses", x => x.PEID);
                    table.ForeignKey(
                        name: "FK_PettyExpnses_BankDeatails_AccountTypeID",
                        column: x => x.AccountTypeID,
                        principalTable: "BankDeatails",
                        principalColumn: "BID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PettyExpnses_ExpenseHeads_EHeadID",
                        column: x => x.EHeadID,
                        principalTable: "ExpenseHeads",
                        principalColumn: "EHID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PettyExpnses_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PettyExpnses_AccountTypeID",
                table: "PettyExpnses",
                column: "AccountTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_PettyExpnses_EHeadID",
                table: "PettyExpnses",
                column: "EHeadID");

            migrationBuilder.CreateIndex(
                name: "IX_PettyExpnses_OrgId",
                table: "PettyExpnses",
                column: "OrgId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PettyExpnses");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "DiscountVal",
                table: "Bills");
        }
    }
}
