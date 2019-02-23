using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations.PaperDb
{
    public partial class AddRelationshipApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sections");

            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "Sections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_InstructorId",
                table: "Sections",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_AspNetUsers_InstructorId",
                table: "Sections",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_AspNetUsers_InstructorId",
                table: "Sections");

            migrationBuilder.DropIndex(
                name: "IX_Sections_InstructorId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "Sections");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Sections",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    IdentityUserId = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Salutation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.UserId);
                });
        }
    }
}
