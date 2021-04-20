using Microsoft.EntityFrameworkCore.Migrations;

namespace PresentationAssignmentApp.Data.Migrations
{
    public partial class UpdatedApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "assignedTeacherId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isTeacher",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_assignedTeacherId",
                table: "AspNetUsers",
                column: "assignedTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_assignedTeacherId",
                table: "AspNetUsers",
                column: "assignedTeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_assignedTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_assignedTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "assignedTeacherId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "isTeacher",
                table: "AspNetUsers");
        }
    }
}
