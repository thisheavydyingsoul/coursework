using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkAdmin.Migrations
{
    /// <inheritdoc />
    public partial class DeviceGameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GameName",
                table: "DevicesGames",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<bool>(
                name: "Installed",
                table: "DevicesGames",
                type: "bit",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Installed",
                table: "DevicesGames");

            migrationBuilder.AlterColumn<string>(
                name: "GameName",
                table: "DevicesGames",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 1);
        }
    }
}
