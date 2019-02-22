using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.WebApp.Models
{
    public class CreateInstructorViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string FirstName { get; set; }

        public String LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Salutation { get; set; }

        public string UserType { get; set; }

    }
}
