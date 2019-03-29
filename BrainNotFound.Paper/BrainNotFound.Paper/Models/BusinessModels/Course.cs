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
        [Required(ErrorMessage = "Please enter a Credit Hour.")]
        [Range(1, 8, ErrorMessage = "Credit Hours must be between 1 and 8.")]
        public int CreditHours { get; set; }
        [Required(ErrorMessage = "Please enter a Course Code")]
        [Range(100, 999, ErrorMessage = "Course Code examples: 101, 202.")]
        public string CourseCode { get; set; } //should this be an int?
        [Required(ErrorMessage = "Please enter a Course Name")]
        [StringLength(128, MinimumLength = 1, ErrorMessage = "Course name examples: Intro To Biology, New Testament Survey.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter the Course Description")]
        [StringLength(1024, MinimumLength = 10, ErrorMessage = "Description must be a minimum of 10 characters.")]
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