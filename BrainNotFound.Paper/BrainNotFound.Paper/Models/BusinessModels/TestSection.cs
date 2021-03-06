using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class TestSection
   {
        //Ask about this model for attributes
	   [Key]
	   public long TestSectionId {get; set;}
	
	    public bool IsQuestionSection {get; set;}
		public int Index {get; set;}
		public string SectionInstructions {get; set;}
		public List<Image> Images {get; set;}
        public string QuestionType { get; set; }
        // Foreign keys
        public long TestId {get; set;}
        [ForeignKey("TestId")]
		public Test Test {get; set;}

        public List<Question> Questions { get; set; }
    }
}