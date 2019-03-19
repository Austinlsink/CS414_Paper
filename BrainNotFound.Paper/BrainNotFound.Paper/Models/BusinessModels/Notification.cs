using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
	public class Notification
	{
		[Key]
		public long NotificationId {get; set;}
		
		public string UserId { get; set;}
        [ForeignKey("UserId")]
		public ApplicationUser ApplicationUser { get; set;}
		
		public string Title { get; set;}
		public string Content { get; set;}
		public string Type { get; set;}
		public DateTime Timestamp {get; set;}
	}
}