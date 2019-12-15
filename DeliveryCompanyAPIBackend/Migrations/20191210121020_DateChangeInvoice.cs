using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryCompanyAPIBackend.Migrations
{
    public partial class DateChangeInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "Invoices");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Invoices",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "data",
                table: "Invoices",
                nullable: false,
                defaultValue: 0);
        }
    }
}
