using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class Essay : Question 
	{
		public string ExpectedAnswer { get; set; }
	}
}