using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bird_Box.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Hours",
                table: "ListeningTasks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenAddedDateTime",
                table: "ListeningTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhenAddedDateTime",
                table: "ListeningTasks");

            migrationBuilder.AlterColumn<int>(
                name: "Hours",
                table: "ListeningTasks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "TEXT");
        }
    }
}
