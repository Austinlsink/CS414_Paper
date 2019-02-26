
using BrainNotFound.Paper.WebApp.Models.BusinessModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.WebApp
{
    public class PaperDbContext : IdentityDbContext<Models.BusinessModels.ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole, string>
    {
        public DbSet<Department> Departments { get; set; } // Example
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionMeetingTime> SectionMeetingTimes { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<StudentTestAssignment> StudentTestAssignments { get; set; }
        public DbSet<TestSection> TestSections { get; set; }
        public DbSet<FieldOfStudy> FieldsOfStudy { get; set; }
        public DbSet<StudentMajor> StudentMajors { get; set; }
        public DbSet<StudentMinor> StudentMinors { get; set; }
        public DbSet<SystemInfo> SystemInfos { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<TrueFalse> TrueFalses { get; set; }
        public DbSet<Essay> Essays { get; set; }
        public DbSet<FillInTheBlank> FillInTheBlanks { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }
        public DbSet<TestSchedule> TestSchedules { get; set; }
        public DbSet<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }


        // Constructor
        public PaperDbContext(DbContextOptions<PaperDbContext> options) 
            : base(options)
        {  }
    }
}