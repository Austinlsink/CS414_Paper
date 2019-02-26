using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class MatchingAnswerSide
	{
		[Key]
		public long MatchingAnswerSideId { get; set; } //primary key
		
		public long QuestionId { get; set; } //foreign key
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

		public string MatchingAnswer  { get; set; }
		
		public List<StudentMatchingAnswer>  StudentMatchingAnswers  { get; set; }
	}
}