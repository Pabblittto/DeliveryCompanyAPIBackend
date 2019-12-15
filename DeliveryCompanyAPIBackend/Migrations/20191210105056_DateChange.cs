using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryCompanyAPIBackend.Migrations
{
    public partial class DateChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Contracts");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Contracts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Contracts");

            migrationBuilder.AddColumn<int>(
                name: "Data",
                table: "Contracts",
                nullable: false,
                defaultValue: 0);
        }
    }
}
