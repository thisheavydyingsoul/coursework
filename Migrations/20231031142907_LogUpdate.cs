using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkAdmin.Migrations
{
    /// <inheritdoc />
    public partial class LogUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Administrators_AdministratorUsername",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "AdministratorUsername",
                table: "Logs",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Administrators_AdministratorUsername",
                table: "Logs",
                column: "AdministratorUsername",
                principalTable: "Administrators",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Administrators_AdministratorUsername",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "AdministratorUsername",
                table: "Logs",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Administrators_AdministratorUsername",
                table: "Logs",
                column: "AdministratorUsername",
                principalTable: "Administrators",
                principalColumn: "Username");
        }
    }
}
