using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
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
		public Test Test {get; set;}
   }
}