using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class ServiceOrProduct_Tbl_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceOrProducts",
                columns: table => new
                {
                    PID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EHId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrProducts", x => x.PID);
                    table.ForeignKey(
                        name: "FK_ServiceOrProducts_ExpenseHeads_EHId",
                        column: x => x.EHId,
                        principalTable: "ExpenseHeads",
                        principalColumn: "EHID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ServiceOrProducts_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceOrProducts_Taxes_TID",
                        column: x => x.TID,
                        principalTable: "Taxes",
                        principalColumn: "TID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrProducts_EHId",
                table: "ServiceOrProducts",
                column: "EHId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrProducts_OrgId",
                table: "ServiceOrProducts",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrProducts_TID",
                table: "ServiceOrProducts",
                column: "TID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceOrProducts");
        }
    }
}
