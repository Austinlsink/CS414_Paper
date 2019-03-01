using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class ChangeRestrictionsInSizeOfStrings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CourseName",
                table: "Courses",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Salutation",
                table: "AspNetUsers",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Courses",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "CourseName",
                table: "Courses",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Salutation",
                table: "AspNetUsers",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);
        }
    }
}
