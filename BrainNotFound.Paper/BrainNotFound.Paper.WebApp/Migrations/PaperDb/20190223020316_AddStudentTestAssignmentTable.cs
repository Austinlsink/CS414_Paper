using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations.PaperDb
{
    public partial class AddStudentTestAssignmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentTestAssignments",
                columns: table => new
                {
                    StudentTestAssignmentId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TestId = table.Column<long>(nullable: false),
                    StudentId = table.Column<string>(nullable: true),
                    Submitted = table.Column<bool>(nullable: false),
                    Grade = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTestAssignments", x => x.StudentTestAssignmentId);
                    table.ForeignKey(
                        name: "FK_StudentTestAssignments_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentTestAssignments_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestAssignments_StudentId",
                table: "StudentTestAssignments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTestAssignments_TestId",
                table: "StudentTestAssignments",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTestAssignments");
        }
    }
}
