using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddStudentMatchingAnswerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentMatchingAnswers",
                columns: table => new
                {
                    StudentMatchingAnswerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMatchingAnswers", x => x.StudentMatchingAnswerId);
                    table.ForeignKey(
                        name: "FK_StudentMatchingAnswers_StudentAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "StudentAnswers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentMatchingAnswers_AnswerId",
                table: "StudentMatchingAnswers",
                column: "AnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentMatchingAnswers");
        }
    }
}
