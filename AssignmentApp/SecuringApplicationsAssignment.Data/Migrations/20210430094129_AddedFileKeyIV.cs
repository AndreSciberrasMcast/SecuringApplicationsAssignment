using Microsoft.EntityFrameworkCore.Migrations;

namespace SecuringApplicationsAssignment.Data.Migrations
{
    public partial class AddedFileKeyIV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IV",
                table: "Submissions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Submissions",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IV",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "Submissions");
        }
    }
}
