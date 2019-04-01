using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class FixedRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestSectionId",
                table: "Questions",
                column: "TestSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_TestSections_TestSectionId",
                table: "Questions",
                column: "TestSectionId",
                principalTable: "TestSections",
                principalColumn: "TestSectionId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_TestSections_TestSectionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TestSectionId",
                table: "Questions");
        }
    }
}
