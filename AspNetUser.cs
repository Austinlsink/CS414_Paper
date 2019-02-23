using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class AspNetUser
	{
		[Key] // Primary key
		public long Id {get; set;}
		
		public string PasswordHash {get; set;}
		public string UserName {get; set;}
		
		// Foreign keys (one-to-one)
		public UserInfo IdentityUser {get; set;}
	}
}