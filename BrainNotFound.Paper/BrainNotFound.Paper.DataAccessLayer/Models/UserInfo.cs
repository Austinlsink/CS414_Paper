using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BrainNotFound.Paper.DataAccessLayer.Models
{
    public class UserInfo
    {
        [Key]
        public long UserId { get; set; }

        public string IdentityUserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }

        /*
        // Instructor relationships
        public List<Section> SectionsTaught { get; set; } // Sections an instructor teaches
        public List<Test> TestsWritten { get; set; } // Tests an instructor has written

        // Student relationships
        public List<StudentAnswer> StudentAnswers { get; set; } // A student's answers on a test
        public List<Enrollment> Enrollments { get; set; }
        public List<StudentTestAssignment> StudentTestAssignments { get; set; }

        */
    }
}
