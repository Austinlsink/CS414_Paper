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

        #region Student Profile and Settings controllers
        [HttpGet, Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var student = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.profile = student;
            return View();
        }

        [HttpGet, Route("Settings")]
        public IActionResult Settings()
        {
            return View();
        }
        #endregion Student profile and Settings controllers


        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("Instructors")]
        public async Task<IActionResult> ViewInstructors()
        {
            // Find the student that is currently logged on
            var student = await _userManager.GetUserAsync(HttpContext.User);

            // Find the instructors that the student has
            var enrollments = _context.Enrollments.Where(e => e.StudentId == student.Id).ToList();
            List<Section> sections = new List<Section>();
            List<ApplicationUser> instructors = new List<ApplicationUser>();
            foreach (Enrollment e in enrollments)
            {
                sections.Add(_context.Sections.Find(e.SectionId));
            }
            foreach (Section s in sections)
            {
                if(!(instructors.Where(i => i.Id == s.InstructorId).Any()))
                    instructors.Add(await _userManager.FindByIdAsync(s.InstructorId));
            }

            // Find the specified user by his username and add him to the ViewBag
            ViewBag.instructorList = instructors;

            return View();
        }

        /// <summary>
        /// Allows the user to view a specific section and its information
        /// </summary>
        /// <param name="code">DepartmentCode + CourseCode</param>
        /// <param name="sectionNumber">section number for the corresonding course</param>
        /// <returns></returns>
        [HttpGet, Route("Sections/View/{code}/{sectionNumber}")]
        public async Task<IActionResult> ViewSection(string code, int sectionNumber)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

            // Find the department associated with the course by DepartmentCode and add it to the ViewBag
            var department = _context.Departments.Where(d => d.DepartmentCode == departmentCode).First();
            ViewBag.department = department;

            // Find the section's course where the CourseCode and DepartmentIds match and add it to the ViewBag 
            var course = _context.Courses.Where(c => c.CourseCode == courseCode && c.DepartmentId == department.DepartmentId).First();
            ViewBag.course = course;

            // Find the section and its information where the CourseIds and SectionNumber match and add it to the ViewBag
            var section = _context.Sections.Where(s => s.CourseId == course.CourseId && s.SectionNumber == sectionNumber).First();
            ViewBag.section = section;

            // Find all of the instructors and add them to the ViewBag
            var instructor = await _userManager.FindByIdAsync(section.InstructorId);
            ViewBag.instructor = instructor;

            // Find all enrollments and add them to the ViewBag
            var enrollment = _context.Enrollments.ToList();
            ViewBag.enrollment = enrollment;

            // Find all the SectionMeetingTimes and add them to the ViewBag
            var sectionMeetingTimeList = _context.SectionMeetingTimes.ToList();
            ViewBag.sectionMeetingTimeList = sectionMeetingTimeList;

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
