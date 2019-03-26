using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class TestSchedule
	{
		 [Key]
		 public long TestScheduleId {get; set;}
		 
        // Ask about what is used for duration
		public int TimeLimit {get; set;}

        public bool IsTimeUnlimited { get; set; }
        [Required]
        [DataType(DataType.Date, ErrorMessage ="Please enter a start time.")]
        public DateTime StartTime {get; set;}
        [Required]
        [DataType(DataType.Date, ErrorMessage = "Please enter an end time.")]
        public DateTime EndTime {get; set;}
		 
		 // Foreign keys
		 public long TestId {get; set;}
        [ForeignKey("TestId")]
		 public Test Test {get; set;}

        public List<StudentAnswer> StudentAnswers { get; set; }
        public List<StudentTestAssignment> StudentTestAssignments { get; set; }
    }
}