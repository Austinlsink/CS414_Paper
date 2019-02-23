using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class TestSchedule
	{
		 [Key]
		 public long TestScheduleId {get; set;}
		 
		 public int Duration {get; set;}
		 public DateTime StartTime {get; set;}
		 public DateTime EndTime {get; set;}
		 
		 // Foreign keys
		 public long TestId {get; set;}
        [ForeignKey("TestId")]
		 public Test Test {get; set;}

        public List<StudentAnswer> StudentAnswers { get; set; }
	}
}