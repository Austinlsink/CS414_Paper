using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
	public class Enrollment
	{
		[Key]
		public long EnrollmentId {get; set;}
		
		public string StudentId { get; set;}
        [ForeignKey("StudentId")]
		public ApplicationUser ApplicationUser { get; set;}
		
		public long SectionId { get; set;}
		public Section Section { get; set;}
		
		public float Grade { get; set;}
	}
}