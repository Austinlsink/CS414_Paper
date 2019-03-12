using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.Models;
using Microsoft.AspNetCore.Authorization;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Identity;

namespace BrainNotFound.Paper.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("Student")]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        public StudentController(
            UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #region Student profile controllers
        [HttpGet, Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var student = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.profile = student;
            return View();
        }

        [HttpGet, Route("Profile/Edit")]
        public IActionResult EditProfile()
        {
            return View();
        }
        #endregion student profile controllers

        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public IActionResult Index()
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

        [HttpGet, Route("Grades")]
        public IActionResult Grades()
        {
            return View();
        }

        

        [HttpGet, Route("Tests")]
        public IActionResult Tests()
        {
            return View();
        }

        [HttpGet, Route("Tests/TestSummary")]
        public IActionResult TestSummary()
        {
            return View();
        }

        [HttpGet, Route("Settings")]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet, Route("Tests/TakeTest")]
        public IActionResult TakeTest()
        {
            return View();
        }

        [HttpGet, Route("Tests/ReviewTest")]
        public IActionResult ReviewTest()
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
