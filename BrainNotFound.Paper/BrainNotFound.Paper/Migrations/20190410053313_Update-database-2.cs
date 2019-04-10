using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class Updatedatabase2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchingAnswerSides_Questions_QuestionId",
                table: "MatchingAnswerSides");

            migrationBuilder.AlterColumn<long>(
                name: "QuestionId",
                table: "MatchingAnswerSides",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_MatchingAnswerSides_Questions_QuestionId",
                table: "MatchingAnswerSides",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchingAnswerSides_Questions_QuestionId",
                table: "MatchingAnswerSides");

            migrationBuilder.AlterColumn<long>(
                name: "QuestionId",
                table: "MatchingAnswerSides",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchingAnswerSides_Questions_QuestionId",
                table: "MatchingAnswerSides",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
