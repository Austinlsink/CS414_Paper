//Student is inheriting Identity 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class Student : ApplicationUser
	{   
	   public string Classification { get; set; }
	   
	   // Standard .NET Core foreign key relationship notation (many side)
	   public long MajorFieldOfStudyId{get; set;}
       [ForeignKey("MajorFieldOfStudyId")]
	   public FieldOfStudy FieldOfStudy1 {get; set;}
	   public long MinorFieldOfStudyId{get; set;}
       [ForeignKey("MinorFieldOfStudyId")]
	   public FieldOfStudy FieldOfStudy2{get; set;}
	}  
}