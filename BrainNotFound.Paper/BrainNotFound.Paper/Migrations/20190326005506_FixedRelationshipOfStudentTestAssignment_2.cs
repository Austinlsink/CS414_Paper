using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class FixedRelationshipOfStudentTestAssignment_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments");

            migrationBuilder.DropIndex(
                name: "IX_StudentTestAssignments_TestId",
                table: "StudentTestAssignments");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "StudentTestAssignments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TestId",
                table: "StudentTestAssignments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestAssignments_TestId",
                table: "StudentTestAssignments",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
