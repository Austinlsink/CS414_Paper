using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class EditTestScheduleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "TestSchedules",
                newName: "TimeLimit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeLimit",
                table: "TestSchedules",
                newName: "Duration");
        }
    }
}
