using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddUpdatedFillInTheBlankQuestionsAndRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FillInTheBlankAnswerGiven",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "FillInTheBlankAnswer",
                table: "Questions");

            migrationBuilder.CreateTable(
                name: "FillInTheBlankQuestions",
                columns: table => new
                {
                    FillInTheBlankQuestionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    WordIndex = table.Column<int>(nullable: false),
                    FillInTheBlankAnswer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FillInTheBlankQuestions", x => x.FillInTheBlankQuestionId);
                    table.ForeignKey(
                        name: "FK_FillInTheBlankQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentFillInTheBlankAnswers",
                columns: table => new
                {
                    StudentFillInTheBlankId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<long>(nullable: false),
                    FillInTheBlankQuestionId = table.Column<long>(nullable: true),
                    AnswerGiven = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFillInTheBlankAnswers", x => x.StudentFillInTheBlankId);
                    table.ForeignKey(
                        name: "FK_StudentFillInTheBlankAnswers_StudentAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "StudentAnswers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFillInTheBlankAnswers_FillInTheBlankQuestions_FillInTheBlankQuestionId",
                        column: x => x.FillInTheBlankQuestionId,
                        principalTable: "FillInTheBlankQuestions",
                        principalColumn: "FillInTheBlankQuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FillInTheBlankQuestions_QuestionId",
                table: "FillInTheBlankQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFillInTheBlankAnswers_AnswerId",
                table: "StudentFillInTheBlankAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFillInTheBlankAnswers_FillInTheBlankQuestionId",
                table: "StudentFillInTheBlankAnswers",
                column: "FillInTheBlankQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentFillInTheBlankAnswers");

            migrationBuilder.DropTable(
                name: "FillInTheBlankQuestions");

            migrationBuilder.AddColumn<string>(
                name: "FillInTheBlankAnswerGiven",
                table: "StudentAnswers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FillInTheBlankAnswer",
                table: "Questions",
                nullable: true);
        }
    }
}
