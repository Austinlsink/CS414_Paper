using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper;

//TODO There is a lot to do


namespace BrainNotFound.Paper.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Models.BusinessModels.ApplicationUser> _userManager;
        private readonly PaperDbContext _context;


        // Constructor
        public AdminController(
            UserManager<Models.BusinessModels.ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Start of Routes
        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("Instructors")]
        public async Task<IActionResult> Instructors()
        {
            var allInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).ToList();

            return View(allInstructors);
        }

        [HttpGet, Route("Instructors/New")]
        public IActionResult NewInstructor()
        {
            return View();
        }

        [HttpPost, Route("Instructors/New")]
        public async Task<IActionResult> NewInstructor(CreateInstructorViewModel model)
        {
            var newInstructor = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Salutation = model.Salutation,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,

            };

            //Create a new Application User
            var result = await _userManager.CreateAsync(newInstructor, model.Password);


            if (result.Succeeded)
            {
                //Fetch created user
                var CreatedUser = await _userManager.FindByEmailAsync(model.Email);

                //Add instructor role to created Application User
                await _userManager.AddToRoleAsync(CreatedUser, "Instructor");

                return RedirectToAction("Instructors", "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ViewData["Message"] += error.Description;
                }
            }

            return View("TestView");
        }

        [HttpGet, Route("Instructors/{Id}")]
        public IActionResult ViewInstructor(String Id)
        {
            ApplicationUser profile = new ApplicationUser()
            {
                Email = "ltesdall@me.com",
                UserName = "LTesdall",
                PhoneNumber = "404897123",
                FirstName = "Lacy",
                LastName = "Tesdall",
                Salutation = "Mrs"
            };

            List<Course> courses = new List<Course>()
            {
                new Course()
                {
                    CourseCode = "CS 306",
                    CourseName = "Database II",
                    CourseId = 1
                },
                new Course()
                {
                    CourseCode = "BI 101",
                    CourseName = "Old Testament Survey",
                    CourseId = 2
                },
                new Course()
                {
                    CourseCode = "EN 126",
                    CourseName = "English Grammar and Composition",
                    CourseId = 3
                }
            };

            List<Section> sections = new List<Section>()
            {
                new Section()
                {
                    CourseId = 1,
                    Capacity = 12,
                    SectionNumber = "1"
                },
                new Section()
                {
                    CourseId = 1,
                    Capacity = 15,
                    SectionNumber = "2"
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 30,
                    SectionNumber = "1"
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 33,
                    SectionNumber = "2"
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 45,
                    SectionNumber = "3"
                },
                new Section()
                {
                    CourseId = 3,
                    Capacity = 32,
                    SectionNumber = "1"
                }

            };

            ViewBag.profile = profile;
            ViewBag.courses = courses;
            ViewBag.sections = sections;

            return View();
        }

        [HttpGet, Route("Instructors/Edit/{Id}")]
        public IActionResult EditInstructor(String Id)
        {
            return View();
        }

        [HttpGet, Route("Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet, Route("Profile/Edit")]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpGet, Route("Students")]
        public IActionResult Students()
        {
            return View();
        }

        [HttpGet, Route("Students/New")]
        public IActionResult NewStudent()
        {
            return View();
        }

        [HttpGet, Route("Students/{Id}")]
        public IActionResult ViewStudent(String Id)
        {
            return View();
        }

        [HttpGet, Route("Students/Edit/{Id}")]
        public IActionResult EditStudent(String Id)
        {
            return View();
        }

        [HttpGet, Route("Department/Edit/{Id}")]
        public IActionResult EditDepartment(long Id)
        {
            var department = _context.Departments.Find(Id);

            ViewBag.department = department;
            return View();
        }

        [HttpPost, Route("Department/Edit/{Id}")]
        public IActionResult EditDepartment(Department dept)
        {

            var depart = _context.Departments.Find(dept.DepartmentId);
            depart.DepartmentCode = dept.DepartmentCode;
            depart.DepartmentName = dept.DepartmentName;

            _context.SaveChanges();

            return RedirectToAction("Departments", "Admin");
        }

        // Display the list of departments
        [HttpGet, Route("Departments")]
        public IActionResult Departments()
        {
            var deparmtents = _context.Departments.ToList();

            return View(deparmtents);
        }

        // Delete a department
        [HttpPost]
        public IActionResult Delete(long id)
        {

            var department = _context.Departments.Find(id);
            _context.Departments.Remove(department);
            _context.SaveChanges();

           return RedirectToAction("Departments", "Admin");
        }


        [HttpGet, Route("Departments/New")]
        public IActionResult NewDepartment()
        {
            return View();
        }

        [HttpPost, Route("Departments/New")]
        public IActionResult NewDepartment(Department model)
        {
            _context.Departments.Add(model);
            _context.SaveChanges();

            ViewData["message"] = "Department code: " + model.DepartmentCode + " " + model.DepartmentName;

            return RedirectToAction("Departments", "Admin");
        }

        [HttpGet, Route("Settings")]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet, Route("Courses")]
        public IActionResult Courses()
        {
            return View();
        }

        [HttpGet, Route("Courses/New")]
        public IActionResult NewCourse()
        {
            return View();
        }

        [HttpGet, Route("Courses/{CourseCode}")]
        public IActionResult ViewCourse(String CourseCode)
        {
            return View();
        }

        [HttpGet, Route("Courses/Edit/{CourseCode}")]
        public IActionResult EditCourse(String CourseCode)
        {
            return View();
        }

        // Why do we have a page that lists all sections?, could that be in the courses page?

        [HttpGet, Route("Sections")]
        public IActionResult Sections()
        {
            return View();
        }

        [HttpGet, Route("Sections/New")]
        public IActionResult NewSection()
        {
            return View();
        }

        [HttpGet, Route("Sections/{CourseCode}/{SectionNumber}")]
        public IActionResult ViewSection(String CourseCode, int SectionNumber)
        {
            return View();
        }

        [HttpGet, Route("Sections/Edit/{CourseCode}/{SectionNumber}")]
        public IActionResult EditSection(String CourseCode, int SectionNumber)
        {
            return View();
        }

    }
}
