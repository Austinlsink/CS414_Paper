using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class QuestionType
	{
		[Key]
		public long QuestionTypeId { get; set; } //private key
		
		public string Name { get; set; }
	}
}