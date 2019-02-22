using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class StudentAnswer
	{
		[Key]
		public long AnswerId {get; set;}
		
		public List<StudentMatchingAnswer> StudentMatchingAnswers {get; set;}
		
		// Foreign keys
		public long QuestionId {get; set;}
		public Question Question {get; set;}
		public long TestScheduleId {get; set;}
		public TestSchedule TestSchedule {get; set;}
		public long UserId {get; set;}
		public UserInfo IdentityUser {get; set;}
	}
}