using System.Collections.Generic;
namespace BrainNotFound.Paper.Services
{
    public static class UserRole
    {
        public static string Admin => "Admin";

        public static string Instructor => "Instructor";

        public static string Student => "Student";

        public static List<string> All
        {
            get
            {
                return new List<string>() { Admin, Instructor, Student };
            }
        }
    }
}