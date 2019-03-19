using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class EditStudentFillInTheBlankAnswersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnswerGiven",
                table: "StudentAnswers",
                newName: "FillInTheBlankAnswerGiven");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FillInTheBlankAnswerGiven",
                table: "StudentAnswers",
                newName: "AnswerGiven");
        }
    }
}
