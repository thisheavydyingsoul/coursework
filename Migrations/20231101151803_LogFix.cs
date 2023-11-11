using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkAdmin.Migrations
{
    /// <inheritdoc />
    public partial class LogFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Offices_OfficeId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_OfficeId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "OfficeId",
                table: "Logs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfficeId",
                table: "Logs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_OfficeId",
                table: "Logs",
                column: "OfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Offices_OfficeId",
                table: "Logs",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");
        }
    }
}
