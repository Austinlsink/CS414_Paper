using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddRelationshipInTestSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "QuestionTypeId",
                table: "TestSections",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TestSections_QuestionTypeId",
                table: "TestSections",
                column: "QuestionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSections_QuestionTypes_QuestionTypeId",
                table: "TestSections",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "QuestionTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSections_QuestionTypes_QuestionTypeId",
                table: "TestSections");

            migrationBuilder.DropIndex(
                name: "IX_TestSections_QuestionTypeId",
                table: "TestSections");

            migrationBuilder.DropColumn(
                name: "QuestionTypeId",
                table: "TestSections");
        }
    }
}
