using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class FieldOfStudy
	{
        // Not sure if there is a required attribute in this file.
		[Key] // Primary key
		public long FieldOfStudyId {get; set;}
		
		public string Name {get; set;}
		
		// Standard .NET Core foreign key relationship notation (one side)
		public long DepartmentId {get; set;}
        [ForeignKey("DepartmentId")]
		public Department Department {get; set;}

        public List<ApplicationUser> StudentsMajoringIn { get; set; }
       //public List<ApplicationUser> StudentsMinoringIn { get; set; }
	}
}