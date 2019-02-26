using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class StudentAnswer
	{
		[Key]
		public long AnswerId {get; set;}
		
		// Foreign keys
		public long QuestionId {get; set;}
        [ForeignKey("QuestionId")]
		public Question Question {get; set;}

		public long TestScheduleId {get; set;}
        [ForeignKey("TestScheduleId")]
		public TestSchedule TestSchedule {get; set;}
		public string StudentId {get; set;}
        [ForeignKey("StudentId")]
		public ApplicationUser ApplicationUser {get; set;}

        public List<StudentMatchingAnswer> StudentMatchingAnswers {get; set;}
        public List<StudentMultipleChoiceAnswer> StudentMultipleChoiceAnswers { get; set; }
    }
}