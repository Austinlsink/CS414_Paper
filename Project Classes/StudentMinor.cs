using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
	public class StudentMinor
	{
		[Key] // Primary key
		public long StudentMinorId {get; set;}
		
		// Foreign keys
		public string StudentId {get; set;}
		[ForeignKey("StudentId")]
		public ApplicationUser ApplicationUser {get; set;}
		public long FieldOfStudyId {get; set;}
		[ForeignKey("FieldOfStudyId")]
		public FieldOfStudy FieldOfStudy {get; set;}
}