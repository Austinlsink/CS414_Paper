using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class TrueFalse : Question
      {
	    public bool Answer { get; set; }	
      }
}