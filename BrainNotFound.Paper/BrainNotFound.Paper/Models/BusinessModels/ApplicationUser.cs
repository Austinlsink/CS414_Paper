using CsvHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using BrainNotFound.Paper;
using Microsoft.AspNetCore.Http;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class ApplicationUser : IdentityUser
    {
        private static string defaultImagePath = "/images/profiles/default.png";
        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(30, MinimumLength = 1)]
        [PersonalData]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(30, MinimumLength = 1)]
        [PersonalData]
        public string LastName { get; set; }
        [StringLength(4, MinimumLength = 1)]
        [PersonalData]
        public string Salutation { get; set; }
        [PersonalData]
        public string Classification { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public string City { get; set; }
        [PersonalData]
        public string State { get; set; }

        [PersonalData]
        public DateTime? DOB { get; set; }
        [PersonalData]
        public string ZipCode { get; set; }

        //Atributes no Maped to the Database
        [NotMapped]
        public string DateOfBirth { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [NotMapped]
        public string ImageURL
        {
            get
            {
                var userImagePath = "/images/profiles/" + UserName + ".png";
                var imageExists = File.Exists("wwwroot" + userImagePath);
                
                return  (imageExists ? userImagePath : defaultImagePath);
             
            }
        }

        //Property added for transferring data from the View to the Controller
        [NotMapped]
        [Required(ErrorMessage = "Please enter a password")]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }


        // Instructor relationships
        public List<Section> SectionsTaught { get; set; } // Sections an instructor teaches

        public List<Test> TestsWritten { get; set; } // Tests an instructor has written

        // Student relationships
        public List<StudentAnswer> StudentAnswers { get; set; } // A student's answers on a test 
        public List<Enrollment> Enrollments { get; set; }
        public List<StudentTestAssignment> StudentTestAssignments { get; set; }
        public List<StudentMajor> StudentMajors { get; set; }
        //public List<StudentMinor> StudentMinors { get; set; }

        public static IEnumerable<ApplicationUser> ParseCsv(string CsvFilePath)
        {
            IEnumerable<ApplicationUser> applicationUsers;

            var reader = new StreamReader(CsvFilePath);
            var csv = new CsvReader(reader);

            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;

            applicationUsers = csv.GetRecords<ApplicationUser>();

            return applicationUsers;
        }
    }
}
