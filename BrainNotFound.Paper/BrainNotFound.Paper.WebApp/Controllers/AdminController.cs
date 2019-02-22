using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.DataAccessLayer.Models;

namespace BrainNotFound.Paper.WebApp.Controllers
{
    [Authorize]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor
        public AdminController(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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
        public IActionResult Instructors()
        {
            return View();
        }

        [HttpGet, Route("Instructors/New")]
        public async Task<IActionResult> NewInstructor()
        {
            ViewData["Errors"] = string.Empty;

            IdentityUser user = new IdentityUser()
            {
                Email = "matthewsnyder1234@gmail.com",
                UserName = "MatthewSnyder"
            };

            var result = await _userManager.CreateAsync(user, "PaperBrain2019!");


            if (result.Succeeded)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ViewData["Errors"] += error.Description;
                }
            }

            return View();
        }

        [HttpGet, Route("Instructors/{Id}")]
        public IActionResult Instructor(String Id)
        {
            return View("ViewInstructor");
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

        // Remove this comment after reading it - Removed Edit Departments - All can be done from this page since there is very little information to deal with
        [HttpGet, Route("Departments")]
        public IActionResult Departments()
        {
            var departments = new List<Department>();

            Department department1 = new Department()
            {
                DepartmentId = 123,
                DepartmentName = "BI"
            };

            Department department2 = new Department()
            {
                DepartmentId = 152,
                DepartmentName = "HI"
            };

            departments.Add(department1);
            departments.Add(department2);

            return View(departments);
        }

        [HttpGet, Route("Departments/New")]
        public IActionResult NewDepartment()
        {
            return View();
        }

        [HttpPost, Route("Departments/New")]
        public IActionResult NewDepartment(Department model)
        {
           
           return View(model.DepartmentCode + model.DepartmentId + model.DepartmentName);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
