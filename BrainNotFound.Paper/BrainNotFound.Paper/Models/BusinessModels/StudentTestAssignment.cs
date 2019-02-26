using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class StudentTestAssignment
	{
		[Key]
		public long StudentTestAssignmentId {get; set;}
		
		public long TestId { get; set; }
        [ForeignKey("TestId")]
		public Test Test { get; set; }
		
		public string StudentId { get; set; }
        [ForeignKey("StudentId")]
		public ApplicationUser ApplicationUser { get; set; }

		public bool Submitted { get; set; }
		public float Grade { get; set; }
	}
}