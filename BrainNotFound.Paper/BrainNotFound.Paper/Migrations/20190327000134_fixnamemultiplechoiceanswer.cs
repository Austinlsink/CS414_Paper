using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class fixnamemultiplechoiceanswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CorrectMultipleChoiceAnswer",
                table: "MultipleChoiceAnswers",
                newName: "MultipleChoiceAnswerOption");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MultipleChoiceAnswerOption",
                table: "MultipleChoiceAnswers",
                newName: "CorrectMultipleChoiceAnswer");
        }
    }
}
