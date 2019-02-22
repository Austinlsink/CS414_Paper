using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class FieldOfStudy
	{
		[Key] // Primary key
		public long FieldOfStudyId {get; set;}
		
		public string Name {get; set;}
		
		// Standard .NET Core foreign key relationship notation (many side)
		public long DepartmentId {get; set;}
		public Department Department {get; set;}
	}
}