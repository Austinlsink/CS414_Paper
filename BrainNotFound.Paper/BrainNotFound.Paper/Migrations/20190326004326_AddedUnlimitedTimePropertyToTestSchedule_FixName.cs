using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddedUnlimitedTimePropertyToTestSchedule_FixName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnlimitedTime",
                table: "TestSchedules",
                newName: "IsTimeUnlimited");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTimeUnlimited",
                table: "TestSchedules",
                newName: "UnlimitedTime");
        }
    }
}
