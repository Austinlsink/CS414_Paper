using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.Models.BusinessModels
{
	public class Course
	{
		[Key] // Primary key
		public long CourseId {get; set;}
		[Required]
        [Range(1, 4, ErrorMessage ="Please enter a credit hour.")]
		public int CreditHours {get; set;}
        [Required]
        [Range(100, 999,  ErrorMessage ="Please enter a course number (example: 101, 202).")]
        public string CourseCode {get; set;} //should this be an int?
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage ="Please enter a course name (example: Intro To Biology, New Testament Survey).")]
		public string CourseName {get; set;}
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Please enter a course description.")]
        public string Description {get; set;}

		public List<Section> Sections {get; set;}
		public List<Test> Tests {get; set;}
		// Standard .NET Core foreign key relationship notation (many side)
		public long DepartmentId {get; set;}
		public Department Department {get; set;}
	}
}