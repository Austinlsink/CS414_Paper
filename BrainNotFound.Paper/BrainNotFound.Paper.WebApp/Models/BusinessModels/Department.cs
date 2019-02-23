using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class Department
    {
        [Key] // Primary key
        public long DepartmentId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1, ErrorMessage ="There must be a Department name entered.")]
        public string DepartmentName { get; set; }
        public List<Course> Courses { get; set; }
        public List<FieldOfStudy> FieldsOfStudy { get; set; }

        public string DepartmentCode { get; set; }
    }
}
