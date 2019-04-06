using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class Question
	{
        [Key]
		public long QuestionId { get; set; } //primary key
		[Required]
		public int Index      { get; set; }
		public int PointValue { get; set; }
        [Required]
		public string Content { get; set; }
        [Required]
        public string QuestionType { get; set; }

        //foreign keys

        public long TestSectionId  { get; set; }
		[ForeignKey("TestSectionId")]
		public TestSection TestSection { get; set; }

        public List<StudentAnswer> StudentAnswers { get; set; }
        public List<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
        public List<MatchingQuestionSide> MatchingQuestionSides { get; set; }
        public List<MatchingAnswerSide>   MatchingAnswerSides   { get; set; }
        public List<FillInTheBlankQuestion>   FillInTheBlankQuestions   { get; set; }
    }
}