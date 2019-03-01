using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.Models.BusinessModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(20, MinimumLength = 1, ErrorMessage ="Please enter a first name.")]
        [PersonalData]
        public string FirstName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Please enter a last name.")]
        [PersonalData]
        public string LastName { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 1, ErrorMessage = "Please enter a salutation (Example: Mr., Mrs., Dr., Miss).")]
        [PersonalData]
        public string Salutation { get; set; }
        [PersonalData]
        public string Classification { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime DOB { get; set; }


        //Atributes no Maped to the Database

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        // Instructor relationships
        public List<Section> SectionsTaught { get; set; } // Sections an instructor teaches

        public List<Test> TestsWritten { get; set; } // Tests an instructor has written

        // Student relationships
        public List<StudentAnswer> StudentAnswers { get; set; } // A student's answers on a test 
        public List<Enrollment> Enrollments { get; set; }
        public List<StudentTestAssignment> StudentTestAssignments { get; set; }
        public List<StudentMajor> StudentMajors { get; set; }
        //public List<StudentMinor> StudentMinors { get; set; }
    }
}
