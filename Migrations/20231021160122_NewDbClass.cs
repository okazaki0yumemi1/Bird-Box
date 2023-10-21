using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bird_Box.Migrations
{
    /// <inheritdoc />
    public partial class NewDbClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "recordingDeviceobjId",
                table: "BirdRecords",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.CreateIndex(
                name: "IX_BirdRecords_recordingDeviceobjId",
                table: "BirdRecords",
                column: "recordingDeviceobjId");

            migrationBuilder.AddForeignKey(
                name: "FK_BirdRecords_InputDevices_recordingDeviceobjId",
                table: "BirdRecords",
                column: "recordingDeviceobjId",
                principalTable: "InputDevices",
                principalColumn: "objId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BirdRecords_InputDevices_recordingDeviceobjId",
                table: "BirdRecords");

            migrationBuilder.DropTable(
                name: "InputDevices");

            migrationBuilder.DropIndex(
                name: "IX_BirdRecords_recordingDeviceobjId",
                table: "BirdRecords");

            migrationBuilder.DropColumn(
                name: "recordingDeviceobjId",
                table: "BirdRecords");
        }
    }
}
