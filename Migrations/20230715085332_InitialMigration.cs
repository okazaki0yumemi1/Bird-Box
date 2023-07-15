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
            migrationBuilder.CreateTable(
                name: "BirdRecords",
                columns: table => new
                {
                    objId = table.Column<Guid>(type: "TEXT", nullable: false),
                    birdName = table.Column<string>(type: "TEXT", nullable: false),
                    detectionThreshold = table.Column<string>(type: "TEXT", nullable: false),
                    recodingDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdRecords", x => x.objId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirdRecords");
        }
    }
}
