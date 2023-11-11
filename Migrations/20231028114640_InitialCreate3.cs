using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWorkAdmin.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Offices_OfficeId",
                table: "Administrators");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "Administrators",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Offices_OfficeId",
                table: "Administrators",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Offices_OfficeId",
                table: "Administrators");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeId",
                table: "Administrators",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Offices_OfficeId",
                table: "Administrators",
                column: "OfficeId",
                principalTable: "Offices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
