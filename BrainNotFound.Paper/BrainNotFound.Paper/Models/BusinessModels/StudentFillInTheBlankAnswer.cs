using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
	public class StudentFillInTheBlankAnswer
	{
        [Key]
        public long StudentFillInTheBlankId { get; set; }

        // Foreign key
        public long AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public StudentAnswer StudentAnswer { get; set; }

        public long? FillInTheBlankQuestionId { get; set; }
        [ForeignKey("FillInTheBlankQuestionId")]
        public FillInTheBlankQuestion FillInTheBlankQuestion { get; set; }

        public string AnswerGiven { get; set; }
    }
}