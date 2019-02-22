using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class StudentFillInTheBlank : StudentAnswer
	{
		public string AnswerGiven {get; set;}
	}
}