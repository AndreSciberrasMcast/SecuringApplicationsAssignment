using Microsoft.EntityFrameworkCore.Migrations;

namespace SecuringApplicationsAssignment.Data.Migrations
{
    public partial class RevertingKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IV",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Submissions");

            migrationBuilder.AddColumn<string>(
                name: "SymmetricIV",
                table: "Submissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SymmetricKey",
                table: "Submissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SymmetricIV",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "SymmetricKey",
                table: "Submissions");

            migrationBuilder.AddColumn<string>(
                name: "IV",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Submissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
