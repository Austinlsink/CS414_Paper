using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class StudentMultipleChoiceAnswer : StudentAnswer
	{
		[Key]
		public long StudentMultipleChoiceAnswerId {get; set;}
		
		// Foreign keys
		public long AnswerId {get; set;}
		[ForeignKey("AnswerId")]
		public StudentAnswer StudentAnswer {get; set;}
		public long MultipleChoiceAnswerId {get; set;}
		[ForeignKey("MultipleChoiceAnswerId")]
		public MultipleChoiceAnswer MultipleChoiceAnswer {get; set;}
	}
}