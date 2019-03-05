
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.Models;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
using System.IO;
using CsvHelper;
using System.Security.Claims;
using BrainNotFound.Paper.Models.ViewModels;

namespace BrainNotFound.Paper.Controllers.DevControllers
{
    public class BimaController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;

        //public IActionResult Run()
        public  IActionResult Run()
        {
            return RedirectToAction("AddSectionsToDb", "Bima");
        }

        [HttpGet, Route("Initialize")]
        public async Task<IActionResult> Initialize()
        {
            return RedirectToAction("AddRolesToDb", "Bima");
        }
    
        // Populates the database with the User Roles
        public async Task<IActionResult> AddRolesToDb()
        {
            string[] roleNames = { "Admin", "Instructor", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            return RedirectToAction("AddAdministratorsToDb", "Bima");
        }

        // Populates the database with the Admistrators Data
        public async Task<IActionResult> AddAdministratorsToDb()
        {
            //Create a Identity User
            ApplicationUser user = new ApplicationUser()
            {
                Email = "abmael.silva@me.com",
                UserName = "AbmaelSilva",
                PhoneNumber = "8502883660",
                FirstName = "Abmael",
                LastName = "Fernandes da Silva",
                Salutation = "Mr"
            };

            //Add the User to the IdentityDbContext
            var result = await _userManager.CreateAsync(user, "PaperBrain2019!");

            //Check if user was created successfully
            if (result.Succeeded)
            {
                //Add the user Role to the created user
                var NewUserFetched = await _userManager.FindByEmailAsync(user.Email);
                await _userManager.AddToRoleAsync(NewUserFetched, "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ViewData["Message"] += error.Description;
                }
            }

            return RedirectToAction("AddInstructorsToDb", "Bima");
        }

        // Populates the Database with the Sample Instructors account
        public async Task<IActionResult> AddInstructorsToDb()
        {
            using (var reader = new StreamReader("SampleData/Instructor_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                var instructors = csv.GetRecords<ApplicationUser>();
                foreach (ApplicationUser instructor in instructors)
                {

                    instructor.UserName = instructor.FirstName + instructor.LastName;

                    //Create a new Application User
                    var result = await _userManager.CreateAsync(instructor, instructor.Password);


                    if (result.Succeeded)
                    {
                        //Fetch created user
                        var CreatedUser = await _userManager.FindByEmailAsync(instructor.Email);

                        //Add instructor role to created Application User
                        await _userManager.AddToRoleAsync(CreatedUser, "Instructor");

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ViewData["Message"] += error.Description;
                        }
                    }
                }
            }

            return RedirectToAction("AddStudentsToDb", "Bima");
        }

        // Populates the database with the sample Departments info
        public async Task<IActionResult> AddDepartmentsToDb()
        {
            var departments = Department.ParseCsv("SampleData/Department_Sample_Data.csv");
           
            foreach (Department department in departments)
            {
                await _context.Departments.AddAsync(department);
            }
            _context.SaveChanges();

            return RedirectToAction("AddStudentsToDb", "Bima");
        }

        // Populates the Database with the Sample Instructors account
        public async Task<IActionResult> AddStudentsToDb()
        {
            Random num = new Random();
            int randomPassword;



            using (var reader = new StreamReader("SampleData/Students_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                var students = csv.GetRecords<ApplicationUser>();
                foreach (ApplicationUser student in students)
                {
                    string[] dob = student.DateOfBirth.Split('/');
                    int year = Int32.Parse(dob[2]);
                    int day = Int32.Parse(dob[1]);
                    int month = Int32.Parse(dob[0]);

                    var date = new DateTime(year, month, day);

                    student.DOB = date;

                    randomPassword = num.Next(100, 1000);
                    student.UserName = student.FirstName + student.LastName;
                    student.Password = student.FirstName + student.LastName + randomPassword.ToString();



                    //Create a new Application User
                    var result = await _userManager.CreateAsync(student, student.Password);


                    if (result.Succeeded)
                    {
                        //Fetch created user
                        var CreatedUser = await _userManager.FindByEmailAsync(student.Email);

                        //Add instructor role to created Application User
                        await _userManager.AddToRoleAsync(CreatedUser, "Student");

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ViewData["Message"] += error.Description;
                        }
                    }
                }
            }

            return RedirectToAction("ForceLogin", "Account");
        }

        public IActionResult AddCoursesToDb()
        {

            using (var reader = new StreamReader("SampleData/Course_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;

                var courses = csv.GetRecords<Course>();


                foreach (Course c in courses)
                {
                    var d = _context.Departments.Where(dd => dd.DepartmentCode == c.DepartmentCode).First();
                    c.Department = d;
                    _context.Courses.Add(c);
                }
                _context.SaveChanges();

            }
            return RedirectToAction("ForceLogin", "Account");
        }


        // Populates the database with the sample Departments info
        public  IActionResult AddSectionsToDb()
        {
            using (var reader = new StreamReader("SampleData/Sections_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                var sections = csv.GetRecords<SampleSection>();
                foreach (var SampleSection in sections)
                {
                    var section = new Section();
                    var sectionMeetingTimes = new List<SectionMeetingTime>();

                    // Finds the course that the section belongs too
                    var courseName = SampleSection.CourseName.Substring(7);
                    var course = _context.Courses.Where(c => c.CourseName == courseName).First();


                    // Assembles the days that the course meets

                    section.TimesMet = SectionMeetingTime.Parse(SampleSection.DaysMet);

                    // Finds the Instructor that teaches this section
                    var instructor = _context.ApplicationUsers.Where(i => i.FirstName == SampleSection.FirstName).First();
                    section.ApplicationUser = instructor;

                    // Sets the Location and capacity of the Section
                    section.Location = SampleSection.Location;
                    section.Capacity = SampleSection.Capacity;

                    // Sets the Section Number
                    var allSectionForCourse = _context.Sections.Where(s => s.CourseId == course.CourseId);
                    int sectionNumber = 0;
                    IQueryable<Section> SectionswithId;

                    bool SectionNumberFound = false;
                    do
                    {
                        sectionNumber += 1;

                         SectionswithId = allSectionForCourse.Where(asfc => asfc.SectionId == sectionNumber);

                        SectionNumberFound = SectionswithId.Any();
                    } while (SectionNumberFound);

                    section.SectionNumber = sectionNumber;
                    section.Course = course;

                    _context.Sections.Add(section);
                        
                }
                _context.SaveChanges();
            }

            return View("TestView");
        }

        // Constructor
        public BimaController(
                    SignInManager<ApplicationUser> signInManager,
                    PaperDbContext context,
                    UserManager<ApplicationUser> userManager,
                    RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

    }
}
