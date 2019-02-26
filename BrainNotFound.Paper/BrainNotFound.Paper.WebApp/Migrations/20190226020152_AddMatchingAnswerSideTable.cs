using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations
{
    public partial class AddMatchingAnswerSideTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchingAnswerSide_Questions_QuestionId",
                table: "MatchingAnswerSide");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMatchingAnswer_StudentAnswers_AnswerId",
                table: "StudentMatchingAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMatchingAnswer_MatchingAnswerSide_MatchingAnswerSideId",
                table: "StudentMatchingAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentMatchingAnswer",
                table: "StudentMatchingAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchingAnswerSide",
                table: "MatchingAnswerSide");

            migrationBuilder.RenameTable(
                name: "StudentMatchingAnswer",
                newName: "StudentMatchingAnswers");

            migrationBuilder.RenameTable(
                name: "MatchingAnswerSide",
                newName: "MatchingAnswerSides");

            migrationBuilder.RenameIndex(
                name: "IX_StudentMatchingAnswer_MatchingAnswerSideId",
                table: "StudentMatchingAnswers",
                newName: "IX_StudentMatchingAnswers_MatchingAnswerSideId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentMatchingAnswer_AnswerId",
                table: "StudentMatchingAnswers",
                newName: "IX_StudentMatchingAnswers_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchingAnswerSide_QuestionId",
                table: "MatchingAnswerSides",
                newName: "IX_MatchingAnswerSides_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentMatchingAnswers",
                table: "StudentMatchingAnswers",
                column: "StudentMatchingAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchingAnswerSides",
                table: "MatchingAnswerSides",
                column: "MatchingAnswerSideId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchingAnswerSides_Questions_QuestionId",
                table: "MatchingAnswerSides",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMatchingAnswers_StudentAnswers_AnswerId",
                table: "StudentMatchingAnswers",
                column: "AnswerId",
                principalTable: "StudentAnswers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMatchingAnswers_MatchingAnswerSides_MatchingAnswerSideId",
                table: "StudentMatchingAnswers",
                column: "MatchingAnswerSideId",
                principalTable: "MatchingAnswerSides",
                principalColumn: "MatchingAnswerSideId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchingAnswerSides_Questions_QuestionId",
                table: "MatchingAnswerSides");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMatchingAnswers_StudentAnswers_AnswerId",
                table: "StudentMatchingAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMatchingAnswers_MatchingAnswerSides_MatchingAnswerSideId",
                table: "StudentMatchingAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentMatchingAnswers",
                table: "StudentMatchingAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchingAnswerSides",
                table: "MatchingAnswerSides");

            migrationBuilder.RenameTable(
                name: "StudentMatchingAnswers",
                newName: "StudentMatchingAnswer");

            migrationBuilder.RenameTable(
                name: "MatchingAnswerSides",
                newName: "MatchingAnswerSide");

            migrationBuilder.RenameIndex(
                name: "IX_StudentMatchingAnswers_MatchingAnswerSideId",
                table: "StudentMatchingAnswer",
                newName: "IX_StudentMatchingAnswer_MatchingAnswerSideId");

            migrationBuilder.RenameIndex(
                name: "IX_StudentMatchingAnswers_AnswerId",
                table: "StudentMatchingAnswer",
                newName: "IX_StudentMatchingAnswer_AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchingAnswerSides_QuestionId",
                table: "MatchingAnswerSide",
                newName: "IX_MatchingAnswerSide_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentMatchingAnswer",
                table: "StudentMatchingAnswer",
                column: "StudentMatchingAnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchingAnswerSide",
                table: "MatchingAnswerSide",
                column: "MatchingAnswerSideId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchingAnswerSide_Questions_QuestionId",
                table: "MatchingAnswerSide",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMatchingAnswer_StudentAnswers_AnswerId",
                table: "StudentMatchingAnswer",
                column: "AnswerId",
                principalTable: "StudentAnswers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMatchingAnswer_MatchingAnswerSide_MatchingAnswerSideId",
                table: "StudentMatchingAnswer",
                column: "MatchingAnswerSideId",
                principalTable: "MatchingAnswerSide",
                principalColumn: "MatchingAnswerSideId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
