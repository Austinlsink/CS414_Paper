using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations
{
    public partial class AddStudentMatchingAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentMatchingAnswer",
                columns: table => new
                {
                    StudentMatchingAnswerId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMatchingAnswer", x => x.StudentMatchingAnswerId);
                    table.ForeignKey(
                        name: "FK_StudentMatchingAnswer_StudentAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "StudentAnswers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentMatchingAnswer_AnswerId",
                table: "StudentMatchingAnswer",
                column: "AnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentMatchingAnswer");
        }
    }
}
