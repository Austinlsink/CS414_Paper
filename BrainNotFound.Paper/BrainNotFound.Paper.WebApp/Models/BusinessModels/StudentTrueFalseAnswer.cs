using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class StudentTrueFalseAnswer : StudentAnswer
	{
		public bool TrueFalseAnswerGiven {get; set;}
	}
}