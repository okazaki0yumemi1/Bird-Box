using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bird_Box.Migrations
{
    /// <inheritdoc />
    public partial class dateTimeAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "recodingDate",
                table: "BirdRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recodingDate",
                table: "BirdRecords");
        }
    }
}
