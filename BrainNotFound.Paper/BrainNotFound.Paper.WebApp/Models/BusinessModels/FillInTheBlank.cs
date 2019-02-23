using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class FillInTheBlank : Question
	{
		public string Answer { get; set; }
	}
}