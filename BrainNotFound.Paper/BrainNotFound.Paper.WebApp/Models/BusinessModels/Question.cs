using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class Question
	{
        // not sure if there should be required attributes in this one.
        [Key]
		public long QuestionId { get; set; } //private key
		
		public int Index      { get; set; }
		public int PointValue { get; set; }
		public string Content { get; set; }
		
		//foreign keys

		public long TestSectionId  { get; set; }
		[ForeignKey("TestSectionId")]
		public TestSection  TestSection;
        public long QuestionTypeId { get; set; }
        [ForeignKey("QuestionTypeId")]
        public QuestionType QuestionType;

        public List<StudentAnswer> StudentAnswers { get; set; }
        //public List<MultipleChoiceAnswer> MultipleChoiceAnswers { get; set; }
        //public List<MatchingQuestionSide> MatchingQuestionSides { get; set; }
        //public List<MatchingAnswerSide>   MatchingAnswerSides   { get; set; }
    }
}