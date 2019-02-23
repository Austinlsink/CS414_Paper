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
		public int    CreditHours {get; set;}
        [Required]
        [StringLength(3, MinimumLength = 3,  ErrorMessage ="The Course must be only 3 numbers")]
        public string CourseCode {get; set;}
		public string CourseName {get; set;}
		public string CourseDescription {get; set;}
		public List<Section> Sections {get; set;}
		// Standard .NET Core foreign key relationship notation (many side)
		public long DepartmentId {get; set;}
		public Department Department {get; set;}
	}
}