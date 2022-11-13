using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplyIO.Migrations
{
    public partial class AddCreateDateToUserMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "User");
        }
    }
}
