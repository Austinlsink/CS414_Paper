using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
	public class Section
	{
		[Key] // Primary key
		public long SectionId {get; set;}
		[Range(1,50)]
		public int    Capacity {get; set;}

        [Required]
        [Range(1,20, ErrorMessage = "Enter a number from 1 to 20.")]
		public string SectionNumber {get; set;}

        [StringLength(7)] 
		public string Location {get; set;}
		public List<SectionMeetingTime> TimesMet {get; set;}
		
		// Foreign keys
		public long CourseId {get; set;}
        [ForeignKey("CourseId")]
        public Course Course {get; set;}

        [ForeignKey("InstructorId")]
        public ApplicationUser ApplicationUser { get; set; }
        public string InstructorId { get; set;} // Instructor ID

        
		public List<Enrollment> Enrollments { get; set;}
	}
}