using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class Course
    {
        [Key] // Primary key
        public long CourseId { get; set; }
        [Required]
        [Range(1, 8, ErrorMessage = "Please enter a credit hour.")]
        public int CreditHours { get; set; }
        [Required]
        [Range(100, 999, ErrorMessage = "Please enter a course number (example: 101, 202).")]
        public string CourseCode { get; set; } //should this be an int?
        [Required]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "Please enter a course name (example: Intro To Biology, New Testament Survey).")]
        public string Name { get; set; }
        [Required]
        [StringLength(1024, MinimumLength = 1, ErrorMessage = "Please enter a course description.")]
        public string Description { get; set; }

        // Properties not mapped
        [NotMapped]
        public string DepartmentCode { get; set; }

        public List<Section> Sections { get; set; }
        public List<Test> Tests { get; set; }
        // Standard .NET Core foreign key relationship notation (many side)
        public long DepartmentId { get; set; }
        public Department Department { get; set; }

        public static IEnumerable ParseCsv(string CsvFilePath)
        {
            IEnumerable<Course> courses;

            var reader = new StreamReader(CsvFilePath);
            var csv = new CsvReader(reader);
            
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;

            courses = csv.GetRecords<Course>();

            return courses;
        }
    }
}