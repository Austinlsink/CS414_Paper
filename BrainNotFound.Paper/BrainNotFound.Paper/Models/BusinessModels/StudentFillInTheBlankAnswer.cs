using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
	public class StudentFillInTheBlankAnswer : StudentAnswer
	{
		public string FillInTheBlankAnswerGiven {get; set;}
	}
}