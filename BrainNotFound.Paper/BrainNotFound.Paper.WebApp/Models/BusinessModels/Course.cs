using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
	public class Course
	{
		[Key] // Primary key
		public long CourseId {get; set;}
		
		public int    CreditHours {get; set;}
		public string CourseCode {get; set;}
		public string CourseName {get; set;}
		public string CourseDescription {get; set;}
		//public List<Section> Sections {get; set;}
		// BELLOOOOOO!!!!
		// Standard .NET Core foreign key relationship notation (many side)
		public long DepartmentId {get; set;}
		public Department Department {get; set;}
	}
}