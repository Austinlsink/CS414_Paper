using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels

{
    public class QuestionType
	{
        // not sure if there should be required attributes in this one.
        [Key]
		public long QuestionTypeId { get; set; } //private key
		
		public string Name { get; set; }

        public List<Question> Questions { get; set; }
	}
}