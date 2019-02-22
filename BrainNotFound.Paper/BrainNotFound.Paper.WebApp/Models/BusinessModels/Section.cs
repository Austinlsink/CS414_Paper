using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
	public class Section
	{
		[Key] // Primary key
		public long SectionId {get; set;}
		
		public int    Capacity {get; set;}
		public string SectionNumber {get; set;}
		public string Location {get; set;}
		//public List<UserInfo> Students {get; set;}
		public List<SectionMeetingTime> TimesMet {get; set;}
		
		// Foreign keys
		public long CourseId {get; set;}
		public Course Course {get; set;}

        [ForeignKey("InstructorId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string InstructorId { get; set;} // Instructor ID

        
		//public List<Enrollment> Enrollments { get; set;}
	}
}