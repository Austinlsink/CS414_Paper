using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class StudentMatchingAnswer
	{
		[Key]
		public long StudentMatchingAnswerId {get; set;}
		
		// Foreign keys
		public long AnswerId {get; set;}
		public StudentAnswer StudentAnswer {get; set;}
		//public long MatchingQuestionSideId {get; set;}
  //      [ForeignKey("MatchingQuestionSideId")]
		//public MatchingQuestionSide MatchingQuestionSide {get; set;}
		public long MatchingAnswerSideId {get; set;}
        [ForeignKey("MatchingAnswerSideId")]
		public MatchingAnswerSide MatchingAnswerSide {get; set;}
	}
}