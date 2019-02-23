using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class Essay : Question 
	{
        // not sure if there should be required attributes in this one.
		public string ExpectedAnswer { get; set; }
	}
}