using Microsoft.EntityFrameworkCore.Migrations;

namespace BrainNotFound.Paper.WebApp.Migrations
{
    public partial class OverrideModelBuilder2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldsOfStudy_Departments_DepartmentId",
                table: "FieldsOfStudy");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_TestSections_TestSectionId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceAnswers_Questions_QuestionId",
                table: "MultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionMeetingTimes_Sections_SectionId",
                table: "SectionMeetingTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_TestSchedules_TestScheduleId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMajors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMajors");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMinors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMinors");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Courses_CourseId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSchedules_Tests_TestId",
                table: "TestSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSections_Tests_TestId",
                table: "TestSections");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldsOfStudy_Departments_DepartmentId",
                table: "FieldsOfStudy",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_TestSections_TestSectionId",
                table: "Image",
                column: "TestSectionId",
                principalTable: "TestSections",
                principalColumn: "TestSectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceAnswers_Questions_QuestionId",
                table: "MultipleChoiceAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "QuestionTypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionMeetingTimes_Sections_SectionId",
                table: "SectionMeetingTimes",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_TestSchedules_TestScheduleId",
                table: "StudentAnswers",
                column: "TestScheduleId",
                principalTable: "TestSchedules",
                principalColumn: "TestScheduleId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMajors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMajors",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "FieldOfStudyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMinors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMinors",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "FieldOfStudyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "AnswerId",
                principalTable: "StudentAnswers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "MultipleChoiceAnswerId",
                principalTable: "MultipleChoiceAnswers",
                principalColumn: "MultipleChoiceAnswerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Courses_CourseId",
                table: "Tests",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSchedules_Tests_TestId",
                table: "TestSchedules",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSections_Tests_TestId",
                table: "TestSections",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_FieldsOfStudy_Departments_DepartmentId",
                table: "FieldsOfStudy");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_TestSections_TestSectionId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_MultipleChoiceAnswers_Questions_QuestionId",
                table: "MultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SectionMeetingTimes_Sections_SectionId",
                table: "SectionMeetingTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAnswers_TestSchedules_TestScheduleId",
                table: "StudentAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMajors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMajors");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMinors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMinors");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Courses_CourseId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSchedules_Tests_TestId",
                table: "TestSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSections_Tests_TestId",
                table: "TestSections");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Sections_SectionId",
                table: "Enrollments",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FieldsOfStudy_Departments_DepartmentId",
                table: "FieldsOfStudy",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_TestSections_TestSectionId",
                table: "Image",
                column: "TestSectionId",
                principalTable: "TestSections",
                principalColumn: "TestSectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MultipleChoiceAnswers_Questions_QuestionId",
                table: "MultipleChoiceAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionTypes_QuestionTypeId",
                table: "Questions",
                column: "QuestionTypeId",
                principalTable: "QuestionTypes",
                principalColumn: "QuestionTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SectionMeetingTimes_Sections_SectionId",
                table: "SectionMeetingTimes",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Courses_CourseId",
                table: "Sections",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_Questions_QuestionId",
                table: "StudentAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAnswers_TestSchedules_TestScheduleId",
                table: "StudentAnswers",
                column: "TestScheduleId",
                principalTable: "TestSchedules",
                principalColumn: "TestScheduleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMajors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMajors",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "FieldOfStudyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMinors_FieldsOfStudy_FieldOfStudyId",
                table: "StudentMinors",
                column: "FieldOfStudyId",
                principalTable: "FieldsOfStudy",
                principalColumn: "FieldOfStudyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_StudentAnswers_AnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "AnswerId",
                principalTable: "StudentAnswers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentMultipleChoiceAnswers_MultipleChoiceAnswers_MultipleChoiceAnswerId",
                table: "StudentMultipleChoiceAnswers",
                column: "MultipleChoiceAnswerId",
                principalTable: "MultipleChoiceAnswers",
                principalColumn: "MultipleChoiceAnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentTestAssignments_Tests_TestId",
                table: "StudentTestAssignments",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Courses_CourseId",
                table: "Tests",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSchedules_Tests_TestId",
                table: "TestSchedules",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSections_Tests_TestId",
                table: "TestSections",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "TestId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
