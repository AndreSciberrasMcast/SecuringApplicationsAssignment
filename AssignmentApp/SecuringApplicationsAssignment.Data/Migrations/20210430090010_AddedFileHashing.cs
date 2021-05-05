using Microsoft.EntityFrameworkCore.Migrations;

namespace SecuringApplicationsAssignment.Data.Migrations
{
    public partial class AddedFileHashing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileHash",
                table: "Submissions",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileHash",
                table: "Submissions");
        }
    }
}
