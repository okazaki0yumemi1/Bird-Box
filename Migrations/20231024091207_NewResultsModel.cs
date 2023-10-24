using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bird_Box.Migrations
{
    /// <inheritdoc />
    public partial class NewResultsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BirdRecords_InputDevices_recordingDeviceobjId",
                table: "BirdRecords");

            migrationBuilder.DropIndex(
                name: "IX_BirdRecords_recordingDeviceobjId",
                table: "BirdRecords");

            migrationBuilder.RenameColumn(
                name: "recordingDeviceobjId",
                table: "BirdRecords",
                newName: "recordingDeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "recordingDeviceId",
                table: "BirdRecords",
                newName: "recordingDeviceobjId");

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
    }
}
