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

        public bool Equals(Department inputDepartment)
        {
            return DepartmentId == inputDepartment.DepartmentId // Id comparison needed?
                && DepartmentName == inputDepartment.DepartmentName
                && DepartmentCode == inputDepartment.DepartmentName;
        }

        // Alternatively...
        //public bool Equals(Department inputDepartment)
        //{
        //    return DepartmentId == inputDepartment.DepartmentId;
        //}
        
        // Alternatively...
        //public bool Equals(Department inputDepartment)
        //{
        //    return DepartmentName == inputDepartment.DepartmentName
        //        && DepartmentCode == inputDepartment.DepartmentCode;
        //}
            // And other combinations of the attributes being compared

        // IMPORTANT! inputDepartment is the one being inserted; this f(x)
        // would be called using the Department IN THE DATABASE!
        public void Update(Department inputDepartment)
        {
            // code for updating the related lists?
            foreach (Course c in Courses)
            {
                if (c.DepartmentId == DepartmentId) // && DepartmentCode condition?
                {
                    c.DepartmentId = inputDepartment.DepartmentId; // id being updated at all?
                    c.DepartmentCode = inputDepartment.DepartmentCode;
                }
            }

            foreach (FieldOfStudy f in FieldsOfStudy)
            {
                if (f.DepartmentId == DepartmentId) // && DepartmentCode condition?
                {
                    f.DepartmentId = inputDepartment.DepartmentId; // Id being updated at all?
                }
            }


            // This specific if statement might not actually end up being used since it's the Id...
            if (DepartmentId != inputDepartment.DepartmentId)
            {
                DepartmentId = inputDepartment.DepartmentId;
            }

            if (DepartmentName != inputDepartment.DepartmentName)
            {
                DepartmentName = inputDepartment.DepartmentName;
            }

            if (DepartmentCode != inputDepartment.DepartmentCode)
            {
                DepartmentCode = inputDepartment.DepartmentCode;
            }

            return;
        }
    }
}
