using System;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.DataAccessLayer.Models
{
	public class SectionMeetingTime
	{
		[Key]
		public long SectionMeetingTimeId {get; set;}
		
		public string Day {get; set;}
		public DateTime StartTime {get; set;}
		public DateTime EndTime {get; set;}
		
		// Foreign keys
		public long SectionId {get; set;}
		public Section Section {get; set;}
	}
}