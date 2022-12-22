using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KINGDOMBANKAPI.API.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PinSalt",
                table: "Accounts",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PinSalt",
                table: "Accounts");
        }
    }
}
