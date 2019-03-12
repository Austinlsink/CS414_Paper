using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddedURLSafeNameToTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URLSafeName",
                table: "Tests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URLSafeName",
                table: "Tests");
        }
    }
}
