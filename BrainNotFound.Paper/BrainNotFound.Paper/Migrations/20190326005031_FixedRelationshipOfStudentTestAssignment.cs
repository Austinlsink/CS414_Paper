using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class FixedRelationshipOfStudentTestAssignment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments");

            migrationBuilder.AlterColumn<long>(
                name: "TestId",
                table: "StudentTestAssignments",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "TestScheduleId",
                table: "StudentTestAssignments",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestAssignments_TestScheduleId",
                table: "StudentTestAssignments",
                column: "TestScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestAssignments_TestSchedules_TestScheduleId",
                table: "StudentTestAssignments",
                column: "TestScheduleId",
                principalTable: "TestSchedules",
                principalColumn: "TestScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestAssignments_TestSchedules_TestScheduleId",
                table: "StudentTestAssignments");

            migrationBuilder.DropIndex(
                name: "IX_StudentTestAssignments_TestScheduleId",
                table: "StudentTestAssignments");

            migrationBuilder.DropColumn(
                name: "TestScheduleId",
                table: "StudentTestAssignments");

            migrationBuilder.AlterColumn<long>(
                name: "TestId",
                table: "StudentTestAssignments",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
