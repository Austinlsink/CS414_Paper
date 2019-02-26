using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddMatchingAnswerSideTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MatchingAnswerSideId",
                table: "StudentMatchingAnswers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "MatchingAnswerSides",
                columns: table => new
                {
                    MatchingAnswerSideId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionId = table.Column<long>(nullable: false),
                    MatchingAnswer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingAnswerSides", x => x.MatchingAnswerSideId);
                    table.ForeignKey(
                        name: "FK_MatchingAnswerSides_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentMatchingAnswers_MatchingAnswerSideId",
                table: "StudentMatchingAnswers",
                column: "MatchingAnswerSideId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingAnswerSides_QuestionId",
                table: "MatchingAnswerSides",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMatchingAnswers_MatchingAnswerSides_MatchingAnswerSideId",
                table: "StudentMatchingAnswers",
                column: "MatchingAnswerSideId",
                principalTable: "MatchingAnswerSides",
                principalColumn: "MatchingAnswerSideId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentMatchingAnswers_MatchingAnswerSides_MatchingAnswerSideId",
                table: "StudentMatchingAnswers");

            migrationBuilder.DropTable(
                name: "MatchingAnswerSides");

            migrationBuilder.DropIndex(
                name: "IX_StudentMatchingAnswers_MatchingAnswerSideId",
                table: "StudentMatchingAnswers");

            migrationBuilder.DropColumn(
                name: "MatchingAnswerSideId",
                table: "StudentMatchingAnswers");
        }
    }
}
