using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    // Not sure if there needs to be attributes in this one
    public class Image
	{
		[Key]
		public long ImageId {get; set;}
		
		public int Index {get; set;}
		public string Comments {get; set;}
		public string ImagePath {get; set;}
		
		// Foreign keys
		public long TestSectionId {get; set;}
        [ForeignKey("TestSectionId")]
		public TestSection TestSection {get; set;}
	}
}