using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.API.Data.Migrations
{
    public partial class addYearsToBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Years",
                table: "Books",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Years",
                table: "Books");

            migrationBuilder.AddColumn<DateTime>(
                name: "Year",
                table: "Books",
                type: "datetime2",
                nullable: true);
        }
    }
}
