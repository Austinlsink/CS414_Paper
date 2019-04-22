using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.Migrations
{
    public partial class AddedDefaultValueForPointsEarned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PointsEarned",
                table: "StudentAnswers",
                nullable: true,
                defaultValue: -1,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PointsEarned",
                table: "StudentAnswers",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true,
                oldDefaultValue: -1);
        }
    }
}
