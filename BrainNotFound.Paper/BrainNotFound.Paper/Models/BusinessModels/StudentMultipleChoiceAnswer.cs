using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class StudentMultipleChoiceAnswer 
	{
        [Key]
        public long StudentMultipleChoiceId { get; set; }

		// Foreign key
        public long AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public StudentAnswer StudentAnswer { get; set; }

		public long? MultipleChoiceAnswerId {get; set;}
        [ForeignKey("MultipleChoiceAnswerId")]
		public MultipleChoiceAnswer MultipleChoiceAnswer {get; set;}
	}
}