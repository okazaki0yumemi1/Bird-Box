using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bird_Box.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "recordingDeviceId",
                table: "BirdRecords",
                newName: "inputDeviceobjId");

            migrationBuilder.CreateIndex(
                name: "IX_BirdRecords_inputDeviceobjId",
                table: "BirdRecords",
                column: "inputDeviceobjId");

            migrationBuilder.AddForeignKey(
                name: "FK_BirdRecords_InputDevices_inputDeviceobjId",
                table: "BirdRecords",
                column: "inputDeviceobjId",
                principalTable: "InputDevices",
                principalColumn: "objId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BirdRecords_InputDevices_inputDeviceobjId",
                table: "BirdRecords");

            migrationBuilder.DropIndex(
                name: "IX_BirdRecords_inputDeviceobjId",
                table: "BirdRecords");

            migrationBuilder.RenameColumn(
                name: "inputDeviceobjId",
                table: "BirdRecords",
                newName: "recordingDeviceId");
        }
    }
}
