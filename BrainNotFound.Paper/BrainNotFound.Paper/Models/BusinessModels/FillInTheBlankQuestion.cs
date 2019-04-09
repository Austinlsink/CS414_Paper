using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    // Ask about if there needs to be an attribute in this one.
    public class FillInTheBlankQuestion
	{
        [Key]
        public long FillInTheBlankQuestionId { get; set; } // private key

        public long QuestionId { get; set; } //foreign key
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public int WordIndex { get; set; }
        public string FillInTheBlankAnswer { get; set; }

        public List<StudentFillInTheBlankAnswer> StudentFillInTheBlankAnswers { get; set; }
    }
}