using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{	
	public class Test
	{
		[Key]
		public long TestId {get; set;}
		
		public bool IsVisible {get; set;} // Whether the test is visible to students or not
		public string TestName {get; set;}
		public List<TestSection> TestSections {get; set;}
		public List<TestSchedule> TestSchedules {get; set;}
		
		// Foreign keys
		public long CourseId {get; set;}
		public Course Course {get; set;}
		public long UserId {get; set;} // ID of instructor who wrote the test
		public UserInfo IdentityUser {get; set;}
		public List<StudentTestAssignment> StudentTestAssignments { get; set; }
	}
}