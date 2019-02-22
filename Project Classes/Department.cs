using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BrainNotFound.Paper.WebApp.Models
{
    public class Department
    {
        [Key] // Primary key
        public long DepartmentId { get; set; }

        public string DepartmentName { get; set; }
        //public List<Course> Courses { get; set; }
    }
    Hello Everyone
}
