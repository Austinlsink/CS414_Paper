using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.WebApp.Models.BusinessModels
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }


        
        // Instructor relationships
        public List<Section> SectionsTaught { get; set; } // Sections an instructor teaches
/*
        public List<Test> TestsWritten { get; set; } // Tests an instructor has written

        // Student relationships
        public List<StudentAnswer> StudentAnswers { get; set; } // A student's answers on a test */
        public List<Enrollment> Enrollments { get; set; }
//        public List<StudentTestAssignment> StudentTestAssignments { get; set; }
    }
}
