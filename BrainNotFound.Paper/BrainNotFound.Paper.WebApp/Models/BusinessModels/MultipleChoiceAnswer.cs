using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class MultipleChoiceAnswer
	{
		[Key]
		public long MultipleChoiceAnswerId { get; set; } // private key
		
		public long QuestionId { get; set; } //foreign key
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
		
		public string Answer  { get; set; }
		public bool IsCorrect { get; set; }
	}
}