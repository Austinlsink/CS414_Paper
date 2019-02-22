using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class Enrollment
	{
		[Key]
		public long EnrollmentId {get; set;}
		
		public long UserId { get; set;}
		public UserInfo IdentityUser { get; set;}
		
		public long SectionId { get; set;}
		public Section Section { get; set;}
		
		public float Grade { get; set;}
	}
}