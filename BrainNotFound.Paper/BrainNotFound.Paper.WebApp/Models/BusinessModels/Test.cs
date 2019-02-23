using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
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
        [ForeignKey("CourseId")]
		public Course Course {get; set;}
		public string InstructorId {get; set;} // ID of instructor who wrote the test
        [ForeignKey("InstructorId")]
        public ApplicationUser ApplicationUser {get; set;}
		public List<StudentTestAssignment> StudentTestAssignments { get; set; }
	}
}