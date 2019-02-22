using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class MatchingAnswerSide
	{
		[Key]
		public long MatchingAnswerSideId { get; set; } //primary key
		
		public long QuestionId { get; set; } //foreign key
		public string Answer  { get; set; }
		
		public List<StudentMatchingAnswer>  StudentMatchingAnswers  { get; set; }
	}
}