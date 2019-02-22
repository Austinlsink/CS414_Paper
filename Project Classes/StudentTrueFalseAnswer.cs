using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class StudentTrueFalseAnswer : StudentAnswer
	{
		public bool AnswerGiven {get; set;}
	}
}