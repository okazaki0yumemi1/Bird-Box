using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bird_Box.Migrations
{
    /// <inheritdoc />
    public partial class NewTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalyzerOptions",
                columns: table => new
                {
                    objId = table.Column<string>(type: "TEXT", nullable: false),
                    latitude = table.Column<string>(type: "TEXT", nullable: true),
                    longitude = table.Column<string>(type: "TEXT", nullable: true),
                    weekOfTheYear = table.Column<string>(type: "TEXT", nullable: true),
                    slist = table.Column<string>(type: "TEXT", nullable: true),
                    sensitivity = table.Column<string>(type: "TEXT", nullable: true),
                    minimumConfidence = table.Column<string>(type: "TEXT", nullable: true),
                    overlapSegments = table.Column<string>(type: "TEXT", nullable: true),
                    cpuThreads = table.Column<string>(type: "TEXT", nullable: true),
                    processingBatchSize = table.Column<string>(type: "TEXT", nullable: true),
                    locale = table.Column<string>(type: "TEXT", nullable: true),
                    speciesFrequencyThreshold = table.Column<string>(type: "TEXT", nullable: true),
                    classifier = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyzerOptions", x => x.objId);
                }
            );

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
                }
            );

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
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ListeningTasks",
                columns: table => new
                {
                    objId = table.Column<string>(type: "TEXT", nullable: false),
                    OutputFolder = table.Column<string>(type: "TEXT", nullable: false),
                    Hours = table.Column<int>(type: "INTEGER", nullable: false),
                    InputDeviceobjId = table.Column<string>(type: "TEXT", nullable: true),
                    OptionsobjId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListeningTasks", x => x.objId);
                    table.ForeignKey(
                        name: "FK_ListeningTasks_AnalyzerOptions_OptionsobjId",
                        column: x => x.OptionsobjId,
                        principalTable: "AnalyzerOptions",
                        principalColumn: "objId",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_ListeningTasks_InputDevices_InputDeviceobjId",
                        column: x => x.InputDeviceobjId,
                        principalTable: "InputDevices",
                        principalColumn: "objId"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_BirdRecords_inputDeviceobjId",
                table: "BirdRecords",
                column: "inputDeviceobjId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ListeningTasks_InputDeviceobjId",
                table: "ListeningTasks",
                column: "InputDeviceobjId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ListeningTasks_OptionsobjId",
                table: "ListeningTasks",
                column: "OptionsobjId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "BirdRecords");

            migrationBuilder.DropTable(name: "ListeningTasks");

            migrationBuilder.DropTable(name: "AnalyzerOptions");

            migrationBuilder.DropTable(name: "InputDevices");
        }
    }
}
