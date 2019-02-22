using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{	
	public class StudentMatchingAnswer
	{
		[Key]
		public long StudentMatchingAnswerId {get; set;}
		
		// Foreign keys
		public long AnswerId {get; set;}
		public StudentAnswer StudentAnswer {get; set;}
		public long MatchingQuestionSideId {get; set;}
		public MatchingQuestionSide MatchingQuestionSide {get; set;}
		public long MatchingAnswerSideId {get; set;}
		public MatchingAnswerSide MatchingAnswerSide {get; set;}
	}
}