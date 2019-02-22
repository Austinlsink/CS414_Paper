using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.WebApp.Models
{
    public class PaperDBContext : DbContext
    {
		public DbSet<AspNetUser> AspNetUsers {get; set;}
		public DbSet<Course> Courses {get; set;}
		public DbSet<Department> Departments {get; set;}
		public DbSet<Enrollment> Enrollments {get; set;}
		public DbSet<Essay> Essays {get; set;}
		public DbSet<FillInTheBlank> FillInTheBlanks  {get; set;}
		public DbSet<UserInfo> IdentityUsers {get; set;}
		public DbSet<Image> Images {get; set;}
		public DbSet<MatchingAnswerSide> MatchingAnswerSides {get; set;}
		public DbSet<MatchingQuestionSide> MatchingQuestionSides {get; set;}
		public DbSet<MultipleChoiceAnswer> MultipleChoiceAnswers {get; set;}
		public DbSet<Question> Questions {get; set;}
		public DbSet<QuestionType> QuestionTypes {get; set;}
		public DbSet<Section> Sections {get; set;}
		public DbSet<SectionMeetingTime> SectionMeetingTimes {get; set;}
		public DbSet<Student> Students {get; set;}
		public DbSet<StudentAnswer> StudentAnswers {get; set;}
		public DbSet<StudentEssayAnswer> StudentEssayAnswers {get; set;}
		public DbSet<StudentFillInTheBlank> StudentFillInTheBlanks {get; set;}
		public DbSet<StudentMatchingAnswer> StudentMatchingAnswers {get; set;}
		public DbSet<StudentMultipleChoiceAnswer> StudentMultipleChoiceAnswers {get; set;}
		public DbSet<StudentTestAssignment> StudentTestAssignments {get; set;}
		public DbSet<StudentTrueFalseAnswer> StudentTrueFalseAnswers {get; set;}
		public DbSet<SystemInfo> SystemInfos {get; set;}
		public DbSet<Test> Tests {get; set;}
		public DbSet<TestSchedule> TestSchedules {get; set;}
		public DbSet<TestSection> TestSections {get; set;}
		public DbSet<TrueFalse> TrueFalses {get; set;}
		
		protected override void OnModelCreating (ModelBuilder modelBuilder)
		{
			//// "AspNetUser"
			//modelBuilder.Entity<AspNetUser>()
			//	.HasOne(p => p.IdentityUser)
			//	.WithOne(i => i.AspNetUser)
			//	.HasForeignKey<UserInfo>(b => b.AspNetUserForeignKey);
			
			//// "Course"
			//modelBuilder.Entity<Course>()
			//	.HasOne(p => p.Department)
			//	.WithMany(b => b.Courses);
				
			//// "Enrollment"
			//modelBuilder.Entity<Enrollment>()
			//	.HasOne(p => p.IdentityUser)
			//	.WithMany(b => b.Enrollments);

			//modelBuilder.Entity<Enrollment>()
			//	.HasOne(p => p.Section)
			//	.WithMany(b => b.Enrollments);
				
			//// "IdentityUser"
			//modelBuilder.Entity<UserInfo>()
			//	.HasOne(p => p.AspNetUser)
			//	.WithOne(i => i.IdentityUser)
			//	.HasForeignKey<AspNetUser>(b => b.IdentityUserForeignKey);
				
			//// "Section"
			//modelBuilder.Entity<Section>()
			//	.HasOne(p => p.Course)
			//	.WithMany(b => b.Sections);
				
			//modelBuilder.Entity<Section>()
			//	.HasOne(p => p.IdentityUser) // The instructor teaching the section
			//	.WithMany(b => b.Sections);			

			////Matt 2nd Start
			////"SectionMeetingTime"
			//modelBuilder.Entity<SectionMeetingTime>()
			//	.HasOne(p => p.Section)
			//	.WithMany(b => b.SectionMeetingTimes);
				
			////"Question"
			//modelBuilder.Entity<Question>()
			//	.HasOne(p => p.QuestionType)
			//	.WithMany(b => b.Questions);
				
			//modelBuilder.Entity<Question>()
			//	.HasOne(p => p.TestSection)
			//	.WithMany(b => b.Questions);
			
			////"MultipleChoiceAnswer"
			//modelBuilder.Entity<MultipleChoiceAnswer>()
			//	.HasOne(p => p.Question)
			//	.WithMany(b => b.MultipleChoiceAnswers);
				
			////"MatchingQuestionSide"
			//modelBuilder.Entity<MatchingQuestionSide>()
			//	.HasOne(p => p.Question)
			//	.WithMany(b => b.MatchingQuestionSides);
				
			//modelBuilder.Entity<MatchingQuestionSide>()
			//	.HasOne(p => p.MatchingAnswerSide)
			//	.WithMany(b => b.MatchingQuestionSides);
			
			////"MatchingAnswerSide"
			//modelBuilder.Entity<MatchingAnswerSide>()
			//	.HasOne(p => p.QuestionId)
			//	.WithMany(b => b.MatchingAnswerSides);	

			////"Image"
			//modelBuilder.Entity<Image>()
			//	.HasOne(p => p.TestSection)
			//	.WithMany(b => b.Images);
			
			//// "StudentAnswer" relationships
			//modelBuilder.Entity<StudentAnswer>()
			//	.HasOne(p => p.Question)
			//	.WithMany(b => b.StudentAnswers);

			//modelBuilder.Entity<StudentAnswer>()
			//	.HasOne(p => p.TestSchedule)
			//	.WithMany(b => b.StudentAnswers);

			//modelBuilder.Entity<StudentAnswer>()
			//	.HasOne(p => p.IdentityUser)
			//	.WithMany(b => b.StudentAnswers);
				
			////Start Matt Snyder
			////"StudentMultipleChoiceAnswer"
			//modelBuilder.Entity<StudentMultipleChoiceAnswer>()
			//.HasOne(p => p.MultipleChoiceAnswer)
			//.WithMany(b => b.StudentMultipleChoiceAnswers);
			
			////"StudentTestAssignment"
			//modelBuilder.Entity<StudentTestAssignment>()
			//.HasOne(p => p.Test)
			//.WithMany(b => b.StudentTestAssignments);
			
			//modelBuilder.Entity<StudentTestAssignment>()
			//.HasOne(p => p.IdentityUser)
			//.WithMany(b => b.StudentTestAssignments);
			
			////"Test" 
			//modelBuilder.Entity<Test>()
			//.HasOne(p => p.Course)
			//.WithMany(b => b.Tests);
			
			//modelBuilder.Entity<Test>()
			//.HasOne(p => p.IdentityUser) // Instructor that wrote the test
			//.WithMany(b => b.Tests);
			
			////"TestSchedule"
			//modelBuilder.Entity<TestSchedule>()
			//.HasOne(p => p.Test)
			//.WithMany(b => b.TestSchedules);
			
			////"TestSection"
			//modelBuilder.Entity<TestSection>()
			//.HasOne(p => p.Test)
			//.WithMany(b => b.TestSections);

		}
    }
}
