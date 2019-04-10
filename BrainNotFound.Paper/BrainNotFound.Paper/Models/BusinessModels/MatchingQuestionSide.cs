using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class MatchingQuestionSide
	{
		[Key]
		public long MatchingQuestionSideId { get; set; } //primary key
		
		public long    QuestionId           { get; set; } //foreign key
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
		public long    MatchingAnswerSideId { get; set; } //foreign key
        [ForeignKey("MatchingAnswerSideId")]
        public MatchingAnswerSide matchingAnswerSide { get; set; }
		public string Content              { get; set; }
		
		public List<StudentMatchingAnswer>  StudentMatchingAnswers  { get; set; }
	}
}