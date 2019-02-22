using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class MatchingQuestionSide
	{
		[Key]
		public long MatchingQuestionSideId { get; set; } //primary key
		
		public long    QuestionId           { get; set; } //foreign key
		public long    MatchingAnswerSideId { get; set; } //foreign key
		public string Content              { get; set; }
		
		public List<StudentMatchingAnswer>  StudentMatchingAnswers  { get; set; }
	}
}