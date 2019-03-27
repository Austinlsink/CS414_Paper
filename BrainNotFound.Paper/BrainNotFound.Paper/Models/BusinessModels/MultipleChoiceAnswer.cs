using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class MultipleChoiceAnswer
	{
		[Key]
		public long MultipleChoiceAnswerId { get; set; } // private key
		
		public long QuestionId { get; set; } //foreign key
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
		
		public string MultipleChoiceAnswerOption  { get; set; }
		public bool IsCorrect { get; set; }

        public List<StudentMultipleChoiceAnswer> StudentMultipleChoiceAnswers { get; set; }
	}
}