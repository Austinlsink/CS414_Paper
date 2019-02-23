using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations.PaperDb
{
    public partial class EditFieldOfStudyApplicationUserTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SectionNumber",
                table: "Sections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Sections",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "SectionMeetingTimes",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Classification",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FieldOfStudyId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FieldOfStudyId1",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FieldOfStudyId",
                table: "AspNetUsers",
                column: "FieldOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FieldOfStudyId1",
                table: "AspNetUsers",
                column: "FieldOfStudyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FieldsOfStudy_FieldOfStudyId",
                table: "AspNetUsers",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "FieldOfStudyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FieldsOfStudy_FieldOfStudyId1",
                table: "AspNetUsers",
                column: "FieldOfStudyId1",
                principalTable: "FieldsOfStudy",
                principalColumn: "FieldOfStudyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FieldsOfStudy_FieldOfStudyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FieldsOfStudy_FieldOfStudyId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FieldOfStudyId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FieldOfStudyId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Classification",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyId1",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SectionNumber",
                table: "Sections",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Sections",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "SectionMeetingTimes",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 9);
        }
    }
}
