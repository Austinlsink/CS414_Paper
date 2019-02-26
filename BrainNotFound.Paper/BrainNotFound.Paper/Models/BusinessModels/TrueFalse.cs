using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class TrueFalse : Question
      {
        //ask about this for required attributes
	    public bool TrueFalseAnswer { get; set; }	
      }
}