using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EduManAPI.Migrations
{
    public partial class StuInfoTbl_Modifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StuFatherImageFile",
                table: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "StuImageFile",
                table: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "StuMotherImageFile",
                table: "StudentInfos");

            migrationBuilder.AddColumn<byte[]>(
                name: "StuFatherImage",
                table: "StudentInfos",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StuFatherImageMimeType",
                table: "StudentInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StuMotherImage",
                table: "StudentInfos",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StuMotherImageMimeType",
                table: "StudentInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentImage",
                table: "StudentInfos",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentImageMimeType",
                table: "StudentInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StuFatherImage",
                table: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "StuFatherImageMimeType",
                table: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "StuMotherImage",
                table: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "StuMotherImageMimeType",
                table: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "StudentImage",
                table: "StudentInfos");

            migrationBuilder.DropColumn(
                name: "StudentImageMimeType",
                table: "StudentInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "StuFatherImageFile",
                table: "StudentInfos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StuImageFile",
                table: "StudentInfos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StuMotherImageFile",
                table: "StudentInfos",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
