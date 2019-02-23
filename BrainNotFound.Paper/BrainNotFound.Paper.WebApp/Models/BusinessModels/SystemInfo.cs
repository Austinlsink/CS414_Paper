using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class SystemInfo
	{
		[Key]
		public long SystemInfoId {get; set;}
		
		public string Attribute {get; set;}
		public string Value {get; set;}
	}
}