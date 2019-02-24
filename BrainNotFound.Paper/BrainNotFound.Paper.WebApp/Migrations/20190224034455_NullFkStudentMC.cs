using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations
{
    public partial class NullFkStudentMC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.AlterColumn<long>(
                name: "MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "MultipleChoiceAnswerId",
                principalTable: "MultipleChoiceAnswers",
                principalColumn: "MultipleChoiceAnswerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.AlterColumn<long>(
                name: "MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "MultipleChoiceAnswerId",
                principalTable: "MultipleChoiceAnswers",
                principalColumn: "MultipleChoiceAnswerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
