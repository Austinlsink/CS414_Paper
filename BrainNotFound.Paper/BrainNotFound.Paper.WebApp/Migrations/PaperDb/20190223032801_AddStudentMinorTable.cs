using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations.PaperDb
{
    public partial class AddStudentMinorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentMinors",
                columns: table => new
                {
                    StudentMinorId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<string>(nullable: true),
                    FieldOfStudyId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMinors", x => x.StudentMinorId);
                    table.ForeignKey(
                        name: "FK_StudentMinors_FieldsOfStudy_FieldOfStudyId",
                        column: x => x.FieldOfStudyId,
                        principalTable: "FieldsOfStudy",
                        principalColumn: "FieldOfStudyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentMinors_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentMinors_FieldOfStudyId",
                table: "StudentMinors",
                column: "FieldOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentMinors_StudentId",
                table: "StudentMinors",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentMinors");
        }
    }
}
