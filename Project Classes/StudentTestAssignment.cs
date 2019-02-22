using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{	
	public class StudentTestAssignment
	{
		[Key]
		public long StudentTestAssignmentId {get; set;}
		
		public long TestId { get; set; }
		public Test Test { get; set; }
		
		public long UserId { get; set; }
		public UserInfo IdentityUser { get; set; }

		public bool Submitted { get; set; }
		public float Grade { get; set; }
	}
}