using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class TestSection
   {
	   [Key]
	   public long TestSectionId {get; set;}
	
	    public bool IsQuestionSection {get; set;}
		public int Index {get; set;}
		public string SectionInstructions {get; set;}
		public List<Image> Images {get; set;}
		
		// Foreign keys
		public long TestId {get; set;}
        [ForeignKey("TestId")]
		public Test Test {get; set;}
   }
}