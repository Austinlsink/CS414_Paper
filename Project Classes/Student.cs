//Student is inheriting Identity 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrainNotFound.Paper.WebApp.Models
{
	public class Student : UserInfo
	{   
	   public string Classification { get; set; }
	   
	   // Standard .NET Core foreign key relationship notation (many side)
	   public long MajorFieldOfStudy{get; set;}
	   public Department Department1 {get; set;}
	   public long MinorFieldOfStudy{get; set;}
	   public Department Department2{get; set;}
	}  
}