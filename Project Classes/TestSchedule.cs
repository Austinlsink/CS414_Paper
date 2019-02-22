using System;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
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
		 public Test Test {get; set;}
	}
}