using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class Bill_Tables_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileGuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileMIME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BID);
                    table.ForeignKey(
                        name: "FK_Bills_ExpenseHeads_ExpHeadId",
                        column: x => x.ExpHeadId,
                        principalTable: "ExpenseHeads",
                        principalColumn: "EHID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bills_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bills_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BillLineItems",
                columns: table => new
                {
                    BLID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProdId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillLineItems", x => x.BLID);
                    table.ForeignKey(
                        name: "FK_BillLineItems_Bills_BID",
                        column: x => x.BID,
                        principalTable: "Bills",
                        principalColumn: "BID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillLineItems_ServiceOrProducts_ProdId",
                        column: x => x.ProdId,
                        principalTable: "ServiceOrProducts",
                        principalColumn: "PID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillLineItems_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "TID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillLineItems_BID",
                table: "BillLineItems",
                column: "BID");

            migrationBuilder.CreateIndex(
                name: "IX_BillLineItems_ProdId",
                table: "BillLineItems",
                column: "ProdId");

            migrationBuilder.CreateIndex(
                name: "IX_BillLineItems_TaxId",
                table: "BillLineItems",
                column: "TaxId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_ExpHeadId",
                table: "Bills",
                column: "ExpHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_OrgId",
                table: "Bills",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_VendorId",
                table: "Bills",
                column: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillLineItems");

            migrationBuilder.DropTable(
                name: "Bills");
        }
    }
}
