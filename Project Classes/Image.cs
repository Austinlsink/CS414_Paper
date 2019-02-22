using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class Image
	{
		[Key]
		public long ImageId {get; set;}
		
		public int Index {get; set;}
		public string Comments {get; set;}
		public string ImagePath {get; set;}
		
		// Foreign keys
		public long TestSectionId {get; set;}
		public TestSection TestSection {get; set;}
	}
}