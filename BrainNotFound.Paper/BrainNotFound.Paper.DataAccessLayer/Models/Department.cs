using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BrainNotFound.Paper.DataAccessLayer.Models
{
    public class Department
    {
        [Key] // Primary key
        // This is not comment anymore jk
        public long DepartmentId { get; set; }

        
        public string DepartmentName { get; set; }
        public List<Course> Courses { get; set; }
        //public List<FieldOfStudy> FieldsOfStudy { get; set; }

        public string DepartmentCode { get; set; }
    }
}
