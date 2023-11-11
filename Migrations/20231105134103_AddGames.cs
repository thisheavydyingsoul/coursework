using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkAdmin.Migrations
{
    /// <inheritdoc />
    public partial class AddGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Offices_OfficeId",
                table: "Devices");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<byte[]>(
                name: "Picture",
                table: "Devices",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "DevicesGames",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    GameName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicesGames", x => new { x.DeviceId, x.GameName });
                    table.ForeignKey(
                        name: "FK_DevicesGames_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DevicesGames_Games_GameName",
                        column: x => x.GameName,
                        principalTable: "Games",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DevicesGames_GameName",
                table: "DevicesGames",
                column: "GameName");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Offices_OfficeId",
                table: "Devices",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Offices_OfficeId",
                table: "Devices");

            migrationBuilder.DropTable(
                name: "DevicesGames");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Devices");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "Devices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Devices",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Offices_OfficeId",
                table: "Devices",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");
        }
    }
}
