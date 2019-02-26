using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
	public class SectionMeetingTime
	{
		[Key]
		public long SectionMeetingTimeId {get; set;}
		[Required(ErrorMessage = "Please enter a day of the week: Monday, Tuesday, Wednesday, Thursday, Friday.")]
        [RegularExpression(@"^(Sun|Mon|(T(ues|hurs))|Fri)(day|\.)?$|Wed(\.|nesday)?$|Sat(\.|urday)?$|T((ue?)|(hu?r?))\.?$", ErrorMessage = "Please enter a day of the week: Monday, Tuesday, Wednesday, Thursday, Friday.")]
        public string Day {get; set;}
 
        [DataType(DataType.Date)]
        public DateTime StartTime {get; set;}
 
        [DataType(DataType.Date)]
        public DateTime EndTime {get; set;}
		
		// Foreign keys
		public long SectionId {get; set;}
        [ForeignKey("SectionId")]
		public Section Section {get; set;}
	}
}