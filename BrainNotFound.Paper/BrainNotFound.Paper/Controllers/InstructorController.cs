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
using BrainNotFound.Paper.Services;

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
        public async Task<IActionResult> Index()
        {
            var instructor = await _userManager.GetUserAsync(HttpContext.User);
            var essayGrading = _context.StudentTestAssignments
                                .Include(x => x.TestSchedule)
                                    .ThenInclude(x => x.Test)
                                .Where(x => x.ManualGradingRequired == true && x.TestSchedule.Test.InstructorId == instructor.Id).ToList();
            ViewBag.EssayGrading = essayGrading;
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
            ApplicationUser instructor = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
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

            ViewBag.instructor = instructor;
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


        #region Courses and Sections Actions
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

            // Find all of the tests for this course
            var instructorTests = _context.TestSchedules.Include(x => x.Test).ThenInclude(x => x.applicationUser).Where(x => x.Test.applicationUser.Id == instructor.Id).ToList();

            var upcomingTests = instructorTests.Where(x => x.EndTime > DateTime.Now).ToList();
            var previousTests = instructorTests.Where(x => x.EndTime < DateTime.Now).ToList();

            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;

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
            var instructorList = await _userManager.GetUsersInRoleAsync("Instructor");
            ViewBag.instructorList = instructorList;

            // Find all of the students and add them to the ViewBag
            var students = await _userManager.GetUsersInRoleAsync("Student");
            ViewBag.students = students;

            // Find all enrollments and add them to the ViewBag
            var enrollment = _context.Enrollments.ToList();
            ViewBag.enrollment = enrollment;

            // Find all the SectionMeetingTimes and add them to the ViewBag
            var sectionMeetingTimeList = _context.SectionMeetingTimes.ToList();
            ViewBag.sectionMeetingTimeList = sectionMeetingTimeList;

            // Find all of the tests for this course
            var instructor = await _userManager.GetUserAsync(HttpContext.User);
            var instructorTests = _context.TestSchedules.Include(x => x.Test).ThenInclude(x => x.applicationUser).Where(x => x.Test.applicationUser.Id == instructor.Id).ToList();

            var upcomingTests = instructorTests.Where(x => x.EndTime > DateTime.Now).ToList();
            var previousTests = instructorTests.Where(x => x.EndTime < DateTime.Now).ToList();

            var unscheduledTests = instructorTests.Except(upcomingTests).Except(previousTests).ToList();

          
            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;
            ViewBag.UnscheduledTests = unscheduledTests;

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


        #endregion Courses and Sections Actions

        #region Test Actions
        //Displays all tests created by an Instructor
        [HttpGet, Route("Tests")]
        public IActionResult Tests()
        {
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            
            // Find all of the previous and upcoming tests for the instructor
            var instructorScheduledTests = _context.TestSchedules.Include(x => x.Test).Where(x => x.Test.applicationUser.Id == instructor.Id).ToList();
            var upcomingTests = _context.TestSchedules.Include(ts => ts.Test).Where(x => x.EndTime > DateTime.Now).Select(tts => tts.Test).Distinct().ToList();
            var previousTests = _context.TestSchedules.Include(ts => ts.Test).Where(x => x.EndTime < DateTime.Now).Select(tts => tts.Test).Distinct().ToList();

            // Find all unscheduled tests
            var instructorTests = _context.Tests.Include(x => x.TestSchedules).Where(x => x.InstructorId == instructor.Id).ToList();
            var unscheduledTests = instructorTests.Except(instructorScheduledTests.Select(x => x.Test)).ToList();



            // Grab the courses and departments
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();

            ViewBag.Courses = courses;
            ViewBag.Departments = departments;
            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;
            ViewBag.UnscheduledTests = unscheduledTests;
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

        [HttpGet, Route("Tests/ViewTest/View/{DepartmentCode}/{CourseCode}/{URLSafeName}")]
        public IActionResult ViewTest(string DepartmentCode, string CourseCode, string URLSafeName)
        {
            // Get Test info
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            var test = _context.Tests
                .Include(t => t.Course)
                    .ThenInclude(c => c.Department)
                .Include(t => t.TestSchedules)
                .Where(t => t.URLSafeName == URLSafeName &&  t.Course.CourseCode == CourseCode &&  t.InstructorId == Instructor.Id)
                .First();
            

            ViewBag.Test = test;
            
            // Grab the test sections for the test
            var testSections = _context.TestSections
                .Include(ts => ts.Questions)
                .Where(x => x.TestId == test.TestId)
                .ToList();

            ViewBag.TestSections = testSections;

            // Grabs all true false questions
            ViewBag.TrueFalseQuestions = _context.TrueFalses
                .Include(tf => tf.TestSection)
                .Where(tf => tf.TestSection.TestId == test.TestId)
                .ToList();
            
            // Grabs all true false questions
            ViewBag.MultipleChoiceQuestions = _context.Questions
                .Include(mc => mc.TestSection)
                .Include(mc => mc.MultipleChoiceAnswers)
                .Where(mc => mc.TestSection.TestId == test.TestId)
                .ToList();

            // Grabs all true false questions
            ViewBag.EssayQuestions = _context.Essays
                .Include(e => e.TestSection)
                .Where(mc => mc.TestSection.TestId == test.TestId)
                .ToList();

            return View();
        }


        // Allows the user to Edit a test information, add and remove sections
        [HttpGet, Route("Tests/Edit/{DepartmentCode}/{CourseCode}/{URLSafeName}")]
        public IActionResult EditTest(string DepartmentCode, string CourseCode, string URLSafeName)
        {
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var department = _context.Departments.Where(d => d.DepartmentCode == DepartmentCode).First();
            var course = _context.Courses.Where(c => c.DepartmentId == department.DepartmentId && c.CourseCode == CourseCode).First();
            var test = _context.Tests.Where(t => t.URLSafeName == URLSafeName && t.CourseId == course.CourseId).First();
            course.DepartmentCode = department.DepartmentCode;
            var sections = _context.Sections.Where(s => s.InstructorId == Instructor.Id && s.CourseId == course.CourseId).ToList();

            ViewBag.Sections = sections;
            ViewBag.Test = test;
            ViewBag.Course = course;
         
            return View();
        }
        #endregion Test Actions

        #region Create Test Partials

        // When you press save on the information, this happens
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

        #endregion Create Test Partials

    }
}
