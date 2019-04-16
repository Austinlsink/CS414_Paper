using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class StudentTestAssignment
	{
		[Key]
		public long StudentTestAssignmentId {get; set;}
		
		public long TestScheduleId { get; set; }
        [ForeignKey("TestScheduleId")]
		public TestSchedule TestSchedule { get; set; }
		
		public string StudentId { get; set; }
        [ForeignKey("StudentId")]
		public ApplicationUser ApplicationUser { get; set; }

        public bool ManualGradingRequired { get; set; } // Added by Kara 3/30
		public bool Submitted { get; set; }
        public bool Signed { get; set; }
		public float Grade { get; set; }

        [NotMapped]
        public int totalPoints { get; set; }
	}
}