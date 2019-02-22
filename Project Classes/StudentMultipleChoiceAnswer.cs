using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class StudentMultipleChoiceAnswer : StudentAnswer
	{
		// Foreign key
		public long MultipleChoiceAnswerId {get; set;}
		public MultipleChoiceAnswer MultipleChoiceAnswer {get; set;}
	}
}