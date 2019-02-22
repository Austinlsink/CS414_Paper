﻿// <auto-generated />
using System;
using BrainNotFound.Paper.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BrainNotFound.Paper.WebApp.Migrations.PaperDb
{
    [DbContext(typeof(PaperDbContext))]
    partial class PaperDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.Course", b =>
                {
                    b.Property<long>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CourseCode");

                    b.Property<string>("CourseDescription");

                    b.Property<string>("CourseName");

                    b.Property<int>("CreditHours");

                    b.Property<long>("DepartmentId");

                    b.HasKey("CourseId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.Department", b =>
                {
                    b.Property<long>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DepartmentCode");

                    b.Property<string>("DepartmentName");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.Section", b =>
                {
                    b.Property<long>("SectionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity");

                    b.Property<long>("CourseId");

                    b.Property<string>("Location");

                    b.Property<string>("SectionNumber");

                    b.Property<long>("UserId");

                    b.HasKey("SectionId");

                    b.HasIndex("CourseId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.SectionMeetingTime", b =>
                {
                    b.Property<long>("SectionMeetingTimeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Day");

                    b.Property<DateTime>("EndTime");

                    b.Property<long>("SectionId");

                    b.Property<DateTime>("StartTime");

                    b.HasKey("SectionMeetingTimeId");

                    b.HasIndex("SectionId");

                    b.ToTable("SectionMeetingTimes");
                });

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.UserInfo", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("IdentityUserId");

                    b.Property<string>("LastName");

                    b.Property<string>("Salutation");

                    b.HasKey("UserId");

                    b.ToTable("UserInfos");
                });

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.Course", b =>
                {
                    b.HasOne("BrainNotFound.Paper.DataAccessLayer.Models.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.Section", b =>
                {
                    b.HasOne("BrainNotFound.Paper.DataAccessLayer.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BrainNotFound.Paper.DataAccessLayer.Models.SectionMeetingTime", b =>
                {
                    b.HasOne("BrainNotFound.Paper.DataAccessLayer.Models.Section", "Section")
                        .WithMany("TimesMet")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
