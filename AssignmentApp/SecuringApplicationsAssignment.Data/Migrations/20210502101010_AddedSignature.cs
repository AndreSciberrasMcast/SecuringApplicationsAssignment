using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SecuringApplicationsAssignment.Data.Migrations
{
    public partial class AddedSignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AlterColumn<string>(
                name: "SymmetricKey",
                table: "Submissions",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SymmetricIV",
                table: "Submissions",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Signature",
                table: "Submissions",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Signature",
                table: "Submissions");

            migrationBuilder.AlterColumn<byte[]>(
                name: "SymmetricKey",
                table: "Submissions",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<byte[]>(
                name: "SymmetricIV",
                table: "Submissions",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string));

           
        }
    }
}
