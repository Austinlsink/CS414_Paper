using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddMatchingQuestionSideTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MatchingQuestionSideId",
                table: "StudentMatchingAnswers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MatchingQuestionSides",
                columns: table => new
                {
                    MatchingQuestionSideId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    MatchingAnswerSideId = table.Column<long>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingQuestionSides", x => x.MatchingQuestionSideId);
                    table.ForeignKey(
                        name: "FK_MatchingQuestionSides_MatchingAnswerSides_MatchingAnswerSideId",
                        column: x => x.MatchingAnswerSideId,
                        principalTable: "MatchingAnswerSides",
                        principalColumn: "MatchingAnswerSideId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MatchingQuestionSides_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentMatchingAnswers_MatchingQuestionSideId",
                table: "StudentMatchingAnswers",
                column: "MatchingQuestionSideId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingQuestionSides_MatchingAnswerSideId",
                table: "MatchingQuestionSides",
                column: "MatchingAnswerSideId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingQuestionSides_QuestionId",
                table: "MatchingQuestionSides",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMatchingAnswers_MatchingQuestionSides_MatchingQuestionSideId",
                table: "StudentMatchingAnswers",
                column: "MatchingQuestionSideId",
                principalTable: "MatchingQuestionSides",
                principalColumn: "MatchingQuestionSideId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentMatchingAnswers_MatchingQuestionSides_MatchingQuestionSideId",
                table: "StudentMatchingAnswers");

            migrationBuilder.DropTable(
                name: "MatchingQuestionSides");

            migrationBuilder.DropIndex(
                name: "IX_StudentMatchingAnswers_MatchingQuestionSideId",
                table: "StudentMatchingAnswers");

            migrationBuilder.DropColumn(
                name: "MatchingQuestionSideId",
                table: "StudentMatchingAnswers");
        }
    }
}
