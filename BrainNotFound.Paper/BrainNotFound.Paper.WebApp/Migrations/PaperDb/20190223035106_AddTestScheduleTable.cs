using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations.PaperDb
{
    public partial class AddTestScheduleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TestScheduleId",
                table: "StudentAnswers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "TestSchedules",
                columns: table => new
                {
                    TestScheduleId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Duration = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    TestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSchedules", x => x.TestScheduleId);
                    table.ForeignKey(
                        name: "FK_TestSchedules_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "TestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentAnswers_TestScheduleId",
                table: "StudentAnswers",
                column: "TestScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSchedules_TestId",
                table: "TestSchedules",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_TestSchedules_TestScheduleId",
                table: "StudentAnswers",
                column: "TestScheduleId",
                principalTable: "TestSchedules",
                principalColumn: "TestScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_TestSchedules_TestScheduleId",
                table: "StudentAnswers");

            migrationBuilder.DropTable(
                name: "TestSchedules");

            migrationBuilder.DropIndex(
                name: "IX_StudentAnswers_TestScheduleId",
                table: "StudentAnswers");

            migrationBuilder.DropColumn(
                name: "TestScheduleId",
                table: "StudentAnswers");
        }
    }
}
