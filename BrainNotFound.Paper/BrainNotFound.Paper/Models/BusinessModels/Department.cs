using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class Department
    {
        [Key] // Primary key
        public long DepartmentId { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1, ErrorMessage ="Please enter a Department name.")]
        public string DepartmentName { get; set; }
        public List<Course> Courses { get; set; }
        public List<FieldOfStudy> FieldsOfStudy { get; set; }

        public string DepartmentCode { get; set; }
    }
}
