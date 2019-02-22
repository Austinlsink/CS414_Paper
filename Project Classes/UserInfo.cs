//2-8-19
//Identity User Starting point
// https://docs.microsoft.com/en-us/ef/core/
// List is for foreign keys example "public List<Post> Posts { get; set; }"

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class UserInfo
	{
		[Key]
		public long UserId { get; set; } //Primary Key
		
		public string Email       { get; set; }
		public string Password    { get; set; }
		public string PhoneNumber { get; set; }
		public string FirstName   { get; set; }
		public string LastName    { get; set; }
		public string Salutation  { get; set; }
		public string UserType    { get; set; }
		
		// Instructor relationships
		public List<Section> SectionsTaught { get; set; } // Sections an instructor teaches
		public List<Test> TestsWritten { get; set; } // Tests an instructor has written
		
		// Student relationships
		public List<StudentAnswer> StudentAnswers{ get; set; } // A student's answers on a test
		public List<Enrollment> Enrollments { get; set; }
		public List<StudentTestAssignment> StudentTestAssignments { get; set; }
		
		// Foreign keys (one-to-one)
		public AspNetUser AspNetUser {get; set;}

	}
}