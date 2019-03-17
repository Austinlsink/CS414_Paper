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
using Microsoft.EntityFrameworkCore;

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
                        if (!(students.Where(s => s.Id == student.Id).Any()))
                        {
                            students.Add(student);
                        }
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
            //ViewData["message"] = code;
            //return View("TestView");

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

        //Displays all tests created by an Instructor
        [HttpGet, Route("Tests")]
        public IActionResult Tests()
        {
            
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var tests = _context.Tests.Where(t => t.InstructorId == Instructor.Id);
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();

            ViewBag.Tests = tests;
            ViewBag.Courses = courses;
            ViewBag.Departments = departments;
            return View();
        }

        // First Step in Creating a test
        [HttpGet, Route("Tests/CreateTest")]
        public IActionResult CreateTest()
        {
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var departments = _context.Departments;
            List<Course> coursesTaught = new List<Course>();
            var sectionsTaught = _context.Sections.Where(S => S.InstructorId == Instructor.Id);

            // Gets all courses tought by instructor removing duplicates from sections
            foreach (var section in sectionsTaught)
            {
                var currentCourse = _context.Courses.Where(c => c.CourseId == section.CourseId).First();
                if (coursesTaught.Contains(currentCourse) == false)
                {
                    currentCourse.DepartmentCode = (departments.Where(d => d.DepartmentId == currentCourse.DepartmentId).First()).DepartmentCode;
                    coursesTaught.Add(currentCourse);

                }
            }
            ViewBag.CoursesTaught = coursesTaught;
            return View();
        }

        // Saves the new test to the database
        [HttpPost, Route("Tests/CreateTest")]
        public IActionResult CreateTest(Test test)
        {
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var course = _context.Courses.Find(test.CourseId);
            var department = _context.Departments.Find(course.DepartmentId);
            test.applicationUser = Instructor;
            test.IsVisible = false;
            test.URLSafeName = test.TestName.Replace(" ", "_");

            _context.Tests.Add(test);
            _context.SaveChanges();

            return RedirectToAction("EditTest", "Instructor", new { DepartmentCode = department.DepartmentCode, CourseCode = course.CourseCode, URLSafeName = test.URLSafeName });
        }

        [HttpGet, Route("Tests/ViewTest")]
        public IActionResult ViewTest()
        {
            return View();
        }


        // Allows the user to Edit a test information, add and remove sections
        [HttpGet, Route("Tests/Edit/{DepartmentCode}/{CourseCode}/{URLSafeName}")]
        public IActionResult EditTest(string DepartmentCode, string CourseCode, string URLSafeName)
        {
            var department = _context.Departments.Where(d => d.DepartmentCode == DepartmentCode).First();
            var course = _context.Courses.Where(c => c.DepartmentId == department.DepartmentId && c.CourseCode == CourseCode).First();
            var test = _context.Tests.Where(t => t.URLSafeName == URLSafeName && t.CourseId == course.CourseId).First();
            course.DepartmentCode = department.DepartmentCode;

            ViewBag.Test = test;
            ViewBag.Course = course;
         
            return View();
        }

        // Gets the partial view being displayed
        [HttpGet, Route("Tests/Partials/EditNameAndCourse/{TestId}")]
        public ActionResult PartialEditNameAndCourse(long TestId)
        {
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var departments = _context.Departments;
            List<Course> coursesTaught = new List<Course>();
            var sectionsTaught = _context.Sections.Where(S => S.InstructorId == instructor.Id);
            
            var test = _context.Tests.Find(TestId);
            ViewBag.Test = test;

            foreach (var section in sectionsTaught)
            {
                var currentCourse = _context.Courses.Where(c => c.CourseId == section.CourseId).First();
                if (coursesTaught.Contains(currentCourse) == false)
                {
                    currentCourse.DepartmentCode = (departments.Where(d => d.DepartmentId == currentCourse.DepartmentId).First()).DepartmentCode;
                    coursesTaught.Add(currentCourse);
                }
            }

            ViewBag.CoursesTaught = coursesTaught;
            return PartialView("~/Views/Instructor/CreateTestPartials/_EditNameAndCourse.cshtml");
        }

        //When you press save on the information, this happens
       [HttpPost, Route("Tests/Partials/EditNameAndCourse")]
       public ActionResult PartialEditNameAndCourse(Test test)
       {
            //grab a copy of the test from the database
            var dbTest = _context.Tests.Find(test.TestId);

            //populate the database test name with the view's test name
            dbTest.TestName = test.TestName;
            dbTest.URLSafeName = dbTest.TestName.Replace(" ", "_");

            //update the course on the database test
            dbTest.CourseId = test.CourseId;

            //Grab the course id from the database test
            var course = _context.Courses.Find(dbTest.CourseId);

            //Grab the department from the database test
            var department = _context.Departments.Find(course.DepartmentId);
            _context.Tests.Update(dbTest);
            _context.SaveChanges();
        
           //When you press save, redirect back to the edit test page
           return RedirectToAction("EditTest", "Instructor", new { DepartmentCode = department.DepartmentCode, CourseCode = course.CourseCode, URLSafeName = dbTest.URLSafeName });
       }

        // Gets the partial view New Schedule
        [HttpGet, Route("Tests/Partials/NewSchedule/{TestId}")]
        public ActionResult PartialNewSchedule(long TestId)
        {
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var test = _context.Tests.Find(TestId);
            var sections = _context.Sections.Where(s => s.CourseId == test.CourseId).ToList();

            ViewBag.Sections = sections;
           // ViewBag.Sections = sections;
            return PartialView("~/Views/Instructor/CreateTestPartials/_NewTestSchedual.cshtml");
        }

        [HttpGet, Route("Tests/Partials/StudentInSectionTable/{SectionId}")]
        public ActionResult PartialStudentInSectionTable(long SectionId)
        {
            var enrollments = _context.Enrollments.Where(e => e.SectionId == SectionId).Include(e => e.ApplicationUser);
            var students = new List<ApplicationUser>();
            
            foreach(Enrollment e in enrollments)
            {
                students.Add(e.ApplicationUser);
            }

            ViewBag.Students = students;
            return PartialView("~/Views/Instructor/CreateTestPartials/_StudentInSectionTable.cshtml");
        }
    }
}