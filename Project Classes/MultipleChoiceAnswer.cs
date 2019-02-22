using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class MultipleChoiceAnswer
	{
		[Key]
		public long MultipleChoiceAnswerId { get; set; } // private key
		
		public long QuestionId { get; set; } //foreign key
		
		public string Answer  { get; set; }
		public bool IsCorrect { get; set; }
	}
}