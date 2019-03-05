using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

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

        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Department code must contain 2 characters")]
        public string DepartmentCode { get; set; }

        public static IEnumerable<Department> ParseCsv(string CsvFilePath)
        {
            IEnumerable<Department> departments;

            using (var reader = new StreamReader(CsvFilePath))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                departments = csv.GetRecords<Department>();
            }

            return departments;
        }
    }
}
