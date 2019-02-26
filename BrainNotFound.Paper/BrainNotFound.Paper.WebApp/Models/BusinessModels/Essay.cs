using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class Essay : Question 
	{
		public string ExpectedEssayAnswer { get; set; }
	}
}