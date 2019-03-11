using System;
using System.Collections.Generic;
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

       // public String[] Days =  { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

        [DataType(DataType.Time)]
        public DateTime StartTime {get; set;}
 
        [DataType(DataType.Time)]
        public DateTime EndTime {get; set;}
		
		// Foreign keys
		public long SectionId {get; set;}
        [ForeignKey("SectionId")]
		public Section Section {get; set;}

        public static List<SectionMeetingTime> Parse(string stringTimesMet)
        {
            var timesMet = new List<SectionMeetingTime>();

            string[] times = stringTimesMet.Split("; ");

            for (int index = 0; index < times.Length; index++)
            {
                string current = times[index];
                string weekday = string.Empty;
                string startTime = string.Empty;
                string endTime = string.Empty;
                int startTimeHour;
                int startTimeMinutes;
                int endTimmesHour;
                int endTimmesMinute;
                weekday = current.Substring(0, current.IndexOf(" "));
                current = current.Substring(current.IndexOf(" ") + 1);

                startTime = (current.Split(" - "))[0];
                endTime = (current.Split(" - "))[1];

                startTimeHour = Int32.Parse(startTime.Substring(0, 2));
                startTimeMinutes = Int32.Parse(startTime.Substring(3));
                endTimmesHour = Int32.Parse(endTime.Substring(0, 2)); ;
                endTimmesMinute = Int32.Parse(endTime.Substring(2));
                var currentMeetingTime = new SectionMeetingTime
                {
                    Day = weekday,
                    StartTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, startTimeHour, startTimeMinutes, 0),
                    EndTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, endTimmesHour, endTimmesMinute, 0)
                };

                timesMet.Add(currentMeetingTime);
            }

            return timesMet;
        }
    }
}