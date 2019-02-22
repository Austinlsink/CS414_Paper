using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.DataAccessLayer.Models
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
		public long UserId {get; set;} // Instructor ID
		//public UserInfo IdentityUser {get; set;}
		//public List<Enrollment> Enrollments { get; set;}
	}
}