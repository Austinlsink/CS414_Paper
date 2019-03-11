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

        #region instructor controllers
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
        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser instructor = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.instructor = instructor;

            return View();
        }

        [HttpPost, Route("Profile/Edit")]
        public async Task<IActionResult> EditProfile(ApplicationUser user)
        {
            ApplicationUser instructor = await _userManager.GetUserAsync(HttpContext.User);
            instructor.Salutation = user.Salutation;
            instructor.FirstName = user.FirstName;
            instructor.LastName = user.LastName;
            instructor.PhoneNumber = user.PhoneNumber;
            instructor.Email = user.Email;
            instructor.Address = user.Address;
            instructor.City = user.City;
            instructor.State = user.State;
            instructor.ZipCode = user.ZipCode;

            await _userManager.UpdateAsync(instructor);
            return RedirectToAction("Profile", "Instructor");
        }
        #endregion instructor controllers


        #region student controllers
        [HttpGet, Route("Students")]
        public async Task<IActionResult> Students()
        {
            ApplicationUser instructor = await _userManager.GetUserAsync(HttpContext.User);

            // Find all of the sections that the instructor teaches
            var sections = _context.Sections.Where(s => s.InstructorId == instructor.Id).ToList();

            // Get the enrollment table
            var enrollment = _context.Enrollments.ToList();

            // Find all of the students that the instructor teaches
            var allStudents = (await _userManager.GetUsersInRoleAsync("Student")).OrderBy(o => o.FirstName).ToList();


            List<ApplicationUser> students = new List<ApplicationUser>();
            foreach(Enrollment e in enrollment)
            {
                foreach(ApplicationUser student in allStudents)
                {
                    if (e.StudentId == student.Id)
                    {
                        students.Add(student);
                    }
                }
            }

            ViewBag.students = students;
            return View();
        }

        [HttpGet, Route("ViewStudentProfile/{username}")]
        public async Task<IActionResult> ViewStudentProfile(String username)
        {
            var student = await _userManager.FindByNameAsync(username);
            var enrollment = _context.Enrollments.Where(e => e.StudentId == student.Id).ToList();
            var allSections = _context.Sections.ToList();
            var allMeetingTimes = _context.SectionMeetingTimes.ToList();
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();
            List<Section> sections = new List<Section>();

            foreach (Enrollment e in enrollment)
            {
                foreach (Section s in allSections)
                {
                    if (e.SectionId == s.SectionId)
                    {
                        sections.Add(s);
                    }
                }
            }

            ViewBag.profile = student;
            ViewBag.sections = sections;
            ViewBag.sectionMeetingTimesList = allMeetingTimes;
            ViewBag.courses = courses;
            ViewBag.departments = departments;

            return View();
        }
        #endregion student controllers

        /// <summary>
        /// Allows the instructor to view a specific course and all of the sections that he teaches
        /// </summary>
        /// <param name="code">DpartmentCode + CourseCode</param>
        [HttpGet, Route("Courses/{code}")]
        public async Task<IActionResult> ViewCourse(String code)
        {
            // Parsing code into the DepartmentCode and the CourseCode
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

            var instructor = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.instructor = instructor;

            // Find the department and add it to the ViewBag
            var department = _context.Departments.Where(d => d.DepartmentCode == departmentCode).First();
            ViewBag.department = department;

            // Find the specified course and add it to the ViewBag
            var course = _context.Courses.Where(c => c.CourseCode == courseCode && c.DepartmentId == department.DepartmentId).First();
            ViewBag.course = course;

            // Find the sections with the same ID as the course and add it to the ViewBag
            var sections = _context.Sections.Where(s => s.CourseId == course.CourseId && s.InstructorId == instructor.Id);
            ViewBag.sectionsList = sections;

            // Find all of the section meeting times and add it to the ViewBag
            var sectionsectionMeetingTimeList = _context.SectionMeetingTimes.ToList();
            ViewBag.sectionMeetingTimeList = sectionsectionMeetingTimeList;

            return View();
        }

        [HttpGet, Route("Courses/{code}/{sectionNumber}")]
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
            var instructor = await _userManager.GetUsersInRoleAsync("Instructor");
            ViewBag.instructorList = instructor;

            // Find all of the students and add them to the ViewBag
            var students = await _userManager.GetUsersInRoleAsync("Student");
            ViewBag.students = students;

            // Find all enrollments and add them to the ViewBag
            var enrollment = _context.Enrollments.ToList();
            ViewBag.enrollment = enrollment;

            // Find all the SectionMeetingTimes and add them to the ViewBag
            var sectionMeetingTimeList = _context.SectionMeetingTimes.ToList();
            ViewBag.sectionMeetingTimeList = sectionMeetingTimeList;
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
