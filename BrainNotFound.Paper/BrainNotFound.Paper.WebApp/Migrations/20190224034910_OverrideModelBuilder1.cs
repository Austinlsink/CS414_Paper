using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations
{
    public partial class OverrideModelBuilder1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers");

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
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "AnswerId",
                principalTable: "StudentAnswers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "MultipleChoiceAnswerId",
                principalTable: "MultipleChoiceAnswers",
                principalColumn: "MultipleChoiceAnswerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.AlterColumn<long>(
                name: "MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "AnswerId",
                principalTable: "StudentAnswers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "MultipleChoiceAnswerId",
                principalTable: "MultipleChoiceAnswers",
                principalColumn: "MultipleChoiceAnswerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
