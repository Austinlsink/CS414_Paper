using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels

{
    public class QuestionType
	{
		[Key]
		public long QuestionTypeId { get; set; } //private key
		
		public string Name { get; set; }

        public List<Question> Questions { get; set; }
	}
}