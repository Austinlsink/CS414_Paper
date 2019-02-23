using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
	public class SectionMeetingTime
	{
		[Key]
		public long SectionMeetingTimeId {get; set;}
		[Required] 
        [StringLength(9, MinimumLength = 6, ErrorMessage = "Please enter a specified day of the week: Monday, Tuesday, Wednesday, Thursday, Friday.")]
        public string Day {get; set;}
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartTime {get; set;}
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndTime {get; set;}
		
		// Foreign keys
		public long SectionId {get; set;}
        [ForeignKey("SectionId")]
		public Section Section {get; set;}
	}
}