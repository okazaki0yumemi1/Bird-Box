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
                name: "InputDevices",
                columns: table => new
                {
                    objId = table.Column<string>(type: "TEXT", nullable: false),
                    deviceId = table.Column<string>(type: "TEXT", nullable: false),
                    deviceInfo = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputDevices", x => x.objId);
                });

            migrationBuilder.CreateTable(
                name: "BirdRecords",
                columns: table => new
                {
                    objId = table.Column<string>(type: "TEXT", nullable: false),
                    birdName = table.Column<string>(type: "TEXT", nullable: false),
                    detectionThreshold = table.Column<string>(type: "TEXT", nullable: false),
                    recodingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    inputDeviceobjId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BirdRecords", x => x.objId);
                    table.ForeignKey(
                        name: "FK_BirdRecords_InputDevices_inputDeviceobjId",
                        column: x => x.inputDeviceobjId,
                        principalTable: "InputDevices",
                        principalColumn: "objId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BirdRecords_inputDeviceobjId",
                table: "BirdRecords",
                column: "inputDeviceobjId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BirdRecords");

            migrationBuilder.DropTable(
                name: "InputDevices");
        }
    }
}
