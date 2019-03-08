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

namespace BrainNotFound.Paper.Controllers
{
    [Authorize(Roles = "Instructor")]
    [Route("Instructor")]
    public class InstructorController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        public InstructorController(
            UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var instructor = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.profile = instructor;
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

        [HttpGet, Route("ViewStudentProfile")]
        public IActionResult ViewStudentProfile()
        {
            return View();
        }

        [HttpGet, Route("Courses")]
        public IActionResult Courses()
        {
            return View();
        }

        [HttpGet, Route("Courses/{CourseCode}")]
        public IActionResult ViewCourse(String CourseCode)
        {
            return View();
        }

        [HttpGet, Route("Courses/{CourseCode}/Sections")]
        public IActionResult ViewSections(String CourseCode)
        {
            return View();
        }

        [HttpGet, Route("Tests")]
        public IActionResult Tests()
        {
            return View();
        }

        [HttpGet, Route("Tests/CreateTest")]
        public IActionResult CreateTest()
        {
            ApplicationUser currentInstructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var course = _context.Courses.Where(c => c.CourseName == "Origins").First();

            Test test1 = new Test
            {
                IsVisible = true,
                TestName = "Midterm",
                applicationUser = currentInstructor,
                Course = course
            };
            _context.Tests.Add(test1);
            _context.SaveChanges();
            return View();
        }

        [HttpGet, Route("Tests/ViewTest")]
        public IActionResult ViewTest()
        {
            return View();
        }

        [HttpGet, Route("Tests/EditTest")]
        public IActionResult EditTest()
        {
            return View();
        }

        [HttpGet, Route("Tests/EssayGrading")]
        public IActionResult EssayGrading()
        {
            return View();
        }

        [HttpGet, Route("Settings")]
        public IActionResult Settings()
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
