using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
	public class Course
	{
		[Key] // Primary key
		public long CourseId {get; set;}
		[Required]
        [Range(1, 4, ErrorMessage ="There must be at least one credit entered.")]
		public int CreditHours {get; set;}
        [Required]
        [Range(100, 999,  ErrorMessage ="The Course must be only 3 numbers (example: 101, 202).")]
        public string CourseCode {get; set;} //should this be an int?
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage ="There must be a course named entered (example: Intro To Biology, New Testament Survey).")]
		public string CourseName {get; set;}
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "There must be a course description entered.")]
        public string CourseDescription {get; set;}
		public List<Section> Sections {get; set;}
		public List<Test> Tests {get; set;}
		// Standard .NET Core foreign key relationship notation (many side)
		public long DepartmentId {get; set;}
		public Department Department {get; set;}
	}
}