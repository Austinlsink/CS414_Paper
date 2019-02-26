﻿// <auto-generated />
using System;
using BrainNotFound.Paper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BrainNotFound.Paper.Migrations
{
    [DbContext(typeof(PaperDbContext))]
    [Migration("20190226193254_AddStudentMatchingAnswerTable")]
    partial class AddStudentMatchingAnswerTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Classification");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<long?>("FieldOfStudyId");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("Salutation")
                        .IsRequired()
                        .HasMaxLength(4);

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("FieldOfStudyId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Course", b =>
                {
                    b.Property<long>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CourseCode")
                        .IsRequired();

                    b.Property<string>("CourseDescription")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<int>("CreditHours");

                    b.Property<long>("DepartmentId");

                    b.HasKey("CourseId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Department", b =>
                {
                    b.Property<long>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DepartmentCode");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Enrollment", b =>
                {
                    b.Property<long>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Grade");

                    b.Property<long>("SectionId");

                    b.Property<string>("StudentId");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("SectionId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.FieldOfStudy", b =>
                {
                    b.Property<long>("FieldOfStudyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DepartmentId");

                    b.Property<string>("Name");

                    b.HasKey("FieldOfStudyId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("FieldsOfStudy");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Image", b =>
                {
                    b.Property<long>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments");

                    b.Property<string>("ImagePath");

                    b.Property<int>("Index");

                    b.Property<long>("TestSectionId");

                    b.HasKey("ImageId");

                    b.HasIndex("TestSectionId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.MultipleChoiceAnswer", b =>
                {
                    b.Property<long>("MultipleChoiceAnswerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CorrectMultipleChoiceAnswer");

                    b.Property<bool>("IsCorrect");

                    b.Property<long>("QuestionId");

                    b.HasKey("MultipleChoiceAnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("MultipleChoiceAnswers");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Question", b =>
                {
                    b.Property<long>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("Index");

                    b.Property<int>("PointValue");

                    b.Property<long>("QuestionTypeId");

                    b.Property<long>("TestSectionId");

                    b.HasKey("QuestionId");

                    b.HasIndex("QuestionTypeId");

                    b.ToTable("Questions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Question");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.QuestionType", b =>
                {
                    b.Property<long>("QuestionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("QuestionTypeId");

                    b.ToTable("QuestionTypes");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Section", b =>
                {
                    b.Property<long>("SectionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity");

                    b.Property<long>("CourseId");

                    b.Property<string>("InstructorId");

                    b.Property<string>("Location")
                        .HasMaxLength(7);

                    b.Property<string>("SectionNumber")
                        .IsRequired();

                    b.HasKey("SectionId");

                    b.HasIndex("CourseId");

                    b.HasIndex("InstructorId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.SectionMeetingTime", b =>
                {
                    b.Property<long>("SectionMeetingTimeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Day")
                        .IsRequired();

                    b.Property<DateTime>("EndTime");

                    b.Property<long>("SectionId");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("SectionMeetingTimeId");

                    b.HasIndex("SectionId");

                    b.ToTable("SectionMeetingTimes");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentAnswer", b =>
                {
                    b.Property<long>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<long>("QuestionId");

                    b.Property<string>("StudentId");

                    b.Property<long>("TestScheduleId");

                    b.HasKey("AnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TestScheduleId");

                    b.ToTable("StudentAnswers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("StudentAnswer");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentMajor", b =>
                {
                    b.Property<long>("StudentMajorId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FieldOfStudyId");

                    b.Property<string>("StudentId");

                    b.HasKey("StudentMajorId");

                    b.HasIndex("FieldOfStudyId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentMajors");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentMatchingAnswer", b =>
                {
                    b.Property<long>("StudentMatchingAnswerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AnswerId");

                    b.HasKey("StudentMatchingAnswerId");

                    b.HasIndex("AnswerId");

                    b.ToTable("StudentMatchingAnswers");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentMultipleChoiceAnswer", b =>
                {
                    b.Property<long>("StudentMultipleChoiceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AnswerId");

                    b.Property<long?>("MultipleChoiceAnswerId");

                    b.HasKey("StudentMultipleChoiceId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("MultipleChoiceAnswerId");

                    b.ToTable("StudentMultipleChoiceAnswers");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentTestAssignment", b =>
                {
                    b.Property<long>("StudentTestAssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Grade");

                    b.Property<string>("StudentId");

                    b.Property<bool>("Submitted");

                    b.Property<long>("TestId");

                    b.HasKey("StudentTestAssignmentId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TestId");

                    b.ToTable("StudentTestAssignments");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.SystemInfo", b =>
                {
                    b.Property<long>("SystemInfoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Attribute");

                    b.Property<string>("Value");

                    b.HasKey("SystemInfoId");

                    b.ToTable("SystemInfos");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Test", b =>
                {
                    b.Property<long>("TestId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CourseId");

                    b.Property<string>("InstructorId");

                    b.Property<bool>("IsVisible");

                    b.Property<string>("TestName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.HasKey("TestId");

                    b.HasIndex("CourseId");

                    b.HasIndex("InstructorId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.TestSchedule", b =>
                {
                    b.Property<long>("TestScheduleId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Duration");

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("StartTime");

                    b.Property<long>("TestId");

                    b.HasKey("TestScheduleId");

                    b.HasIndex("TestId");

                    b.ToTable("TestSchedules");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.TestSection", b =>
                {
                    b.Property<long>("TestSectionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Index");

                    b.Property<bool>("IsQuestionSection");

                    b.Property<string>("SectionInstructions");

                    b.Property<long>("TestId");

                    b.HasKey("TestSectionId");

                    b.HasIndex("TestId");

                    b.ToTable("TestSections");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Essay", b =>
                {
                    b.HasBaseType("BrainNotFound.Paper.Models.BusinessModels.Question");

                    b.Property<string>("ExpectedEssayAnswer");

                    b.HasDiscriminator().HasValue("Essay");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.FillInTheBlank", b =>
                {
                    b.HasBaseType("BrainNotFound.Paper.Models.BusinessModels.Question");

                    b.Property<string>("FillInTheBlankAnswer");

                    b.HasDiscriminator().HasValue("FillInTheBlank");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.TrueFalse", b =>
                {
                    b.HasBaseType("BrainNotFound.Paper.Models.BusinessModels.Question");

                    b.Property<bool>("TrueFalseAnswer");

                    b.HasDiscriminator().HasValue("TrueFalse");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentEssayAnswer", b =>
                {
                    b.HasBaseType("BrainNotFound.Paper.Models.BusinessModels.StudentAnswer");

                    b.Property<string>("EssayAnswerGiven");

                    b.HasDiscriminator().HasValue("StudentEssayAnswer");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentTrueFalseAnswer", b =>
                {
                    b.HasBaseType("BrainNotFound.Paper.Models.BusinessModels.StudentAnswer");

                    b.Property<bool>("TrueFalseAnswerGiven");

                    b.HasDiscriminator().HasValue("StudentTrueFalseAnswer");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.FieldOfStudy")
                        .WithMany("StudentsMajoringIn")
                        .HasForeignKey("FieldOfStudyId");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Course", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Enrollment", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Section", "Section")
                        .WithMany("Enrollments")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", "ApplicationUser")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.FieldOfStudy", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Department", "Department")
                        .WithMany("FieldsOfStudy")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Image", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.TestSection", "TestSection")
                        .WithMany("Images")
                        .HasForeignKey("TestSectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.MultipleChoiceAnswer", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Question", "Question")
                        .WithMany("MultipleChoiceAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Question", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.QuestionType")
                        .WithMany("Questions")
                        .HasForeignKey("QuestionTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Section", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Course", "Course")
                        .WithMany("Sections")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", "ApplicationUser")
                        .WithMany("SectionsTaught")
                        .HasForeignKey("InstructorId");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.SectionMeetingTime", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Section", "Section")
                        .WithMany("TimesMet")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentAnswer", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Question", "Question")
                        .WithMany("StudentAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", "ApplicationUser")
                        .WithMany("StudentAnswers")
                        .HasForeignKey("StudentId");

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.TestSchedule", "TestSchedule")
                        .WithMany("StudentAnswers")
                        .HasForeignKey("TestScheduleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentMajor", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.FieldOfStudy", "FieldOfStudy")
                        .WithMany()
                        .HasForeignKey("FieldOfStudyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", "ApplicationUser")
                        .WithMany("StudentMajors")
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentMatchingAnswer", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.StudentAnswer", "StudentAnswer")
                        .WithMany("StudentMatchingAnswers")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentMultipleChoiceAnswer", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.StudentAnswer", "StudentAnswer")
                        .WithMany("StudentMultipleChoiceAnswers")
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.MultipleChoiceAnswer", "MultipleChoiceAnswer")
                        .WithMany("StudentMultipleChoiceAnswers")
                        .HasForeignKey("MultipleChoiceAnswerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.StudentTestAssignment", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", "ApplicationUser")
                        .WithMany("StudentTestAssignments")
                        .HasForeignKey("StudentId");

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Test", "Test")
                        .WithMany("StudentTestAssignments")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.Test", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Course", "Course")
                        .WithMany("Tests")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser", "ApplicationUser")
                        .WithMany("TestsWritten")
                        .HasForeignKey("InstructorId");
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.TestSchedule", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Test", "Test")
                        .WithMany("TestSchedules")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.Models.BusinessModels.TestSection", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.Test", "Test")
                        .WithMany("TestSections")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("BrainNotFound.Paper.Models.BusinessModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
