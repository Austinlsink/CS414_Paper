using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    // Ask about if there needs to be an attribute in this one.
    public class FillInTheBlank : Question
	{
		public string Answer { get; set; }
	}
}