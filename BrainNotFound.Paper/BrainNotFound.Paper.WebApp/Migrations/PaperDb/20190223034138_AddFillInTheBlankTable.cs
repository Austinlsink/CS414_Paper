using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations.PaperDb
{
    public partial class AddFillInTheBlankTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TrueFalse_Answer",
                table: "Questions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrueFalse_Answer",
                table: "Questions");

            migrationBuilder.AlterColumn<bool>(
                name: "Answer",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
