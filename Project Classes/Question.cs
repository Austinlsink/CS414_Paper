using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class Question
	{
		[Key]
		public long QuestionId { get; set; } //private key
		
		public int Index      { get; set; }
		public int PointValue { get; set; }
		public string Content { get; set; }
		
		//foreign keys
		public long QuestionTypeId { get; set; }
		public long TestSectionId  { get; set; }
		
		public TestSection  TestSection;
		public QuestionType QuestionType;
		public List<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
		public List<MatchingQuestionSide> MatchingQuestionSides { get; set; }
		public List<MatchingAnswerSide>   MatchingAnswerSides   { get; set; }
	}
}