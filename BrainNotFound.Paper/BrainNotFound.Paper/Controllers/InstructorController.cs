using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;

namespace BrainNotFound.Paper.Controllers
{
    [Authorize(Roles = "Instructor")]
    [Route("Instructor")]
    public class InstructorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        #region instructor controllers

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager">Gives access to all of the user profiles</param>
        /// <param name="context">Gives access to the databse</param>
        public InstructorController(
            UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Allows the instructor to view his dashboard, and gives him quick access to certain items
        /// </summary>
        /// <returns>Index view</returns>
        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public async Task<IActionResult> Index()
        {
            var instructor = await _userManager.GetUserAsync(HttpContext.User);

            var gradingTests = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).Where(x => x.TestSchedule.Test.InstructorId == instructor.Id && x.ManualGradingRequired).Select(x => x.TestSchedule).Include(x => x.StudentTestAssignments).Distinct().ToList();


            var studentEssays = _context.StudentEssayAnswers.Include(x => x.TestSchedule).ThenInclude(x => x.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.PointsEarned == -1 && x.TestSchedule.Test.InstructorId == instructor.Id).Distinct().ToList();

            ViewBag.EssayGrading = gradingTests;
            ViewBag.Tests = _context.Tests.Where(x => x.InstructorId == instructor.Id).ToList();
            return View();
        }

        [HttpGet, Route("Chart/{TestId}")]
        public IActionResult Chart(long TestId)
        {
           ViewBag.Test = _context.Tests.Where(x => x.TestId == TestId).First();
            return PartialView();
        }

        /// <summary>
        /// Allows the instructor to view his profile
        /// </summary>
        /// <returns>Profile View</returns>
        [HttpGet, Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var instructor = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.profile = instructor;

            return View();
        }

        /// <summary>
        /// Allows the instrutor to edit his profile
        /// </summary>
        /// <returns>EditProfile view</returns>
        [HttpGet, Route("Profile/Edit")]
        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser instructor = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.instructor = instructor;

            return View();
        }

        /// <summary>
        /// Saves the edited changes to the instructor's profile
        /// </summary>
        /// <param name="user">ApplicationUser object that contains the new information</param>
        /// <returns>Redirects to the Profile View</returns>
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

        /// <summary>
        /// Allows the instructor to view all of the students enrolled in his sections
        /// </summary>
        /// <returns>Students View</returns>
        [HttpGet, Route("Students")]
        public async Task<IActionResult> Students()
        {
            ApplicationUser instructor = await _userManager.GetUserAsync(HttpContext.User);

            // Find all of the sections that the instructor teaches
            var sections = _context.Sections.Where(s => s.InstructorId == instructor.Id).ToList();
            
            if(sections.Count == 0)
            {
                ViewBag.students = null;
            }
            else
            {
                // Get the enrollment table
                var enrollment = _context.Enrollments.ToList();

                // Find all of the students that the instructor teaches
                var allStudents = (await _userManager.GetUsersInRoleAsync("Student")).ToList();

                List<ApplicationUser> students = new List<ApplicationUser>();
                foreach (Enrollment e in enrollment)
                {
                    foreach (ApplicationUser student in allStudents)
                    {
                        if (e.StudentId == student.Id && sections.Any(x => x.SectionId == e.SectionId))
                        {
                            if (!(students.Where(s => s.Id == student.Id).Any()))
                            {
                                students.Add(student);
                            }
                        }
                    }
                }

                ViewBag.students = students;
            }
            
            return View();
        }

        /// <summary>
        /// Allows the instructor to view a specific student's profile
        /// </summary>
        /// <param name="username">Search criteria for finding the specific student</param>
        /// <returns>ViewStudentProfile view</returns>
        [HttpGet, Route("ViewStudentProfile/{username}")]
        public async Task<IActionResult> ViewStudentProfile(String username)
        {
            ApplicationUser instructor = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            ApplicationUser student = await _userManager.FindByNameAsync(username);
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

        #region Courses and Sections Actions
        /// <summary>
        /// Allows the instructor to view a specific course and all of the sections that he teaches
        /// </summary>
        /// <param name="code">DpartmentCode + CourseCode</param>
        /// <returns>ViewCourse View</returns>
        [HttpGet, Route("Courses/{code}")]
        public async Task<IActionResult> ViewCourse(String code)
        {
            // Parsing code into the DepartmentCode and the CourseCode
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

            ApplicationUser instructor = await _userManager.GetUserAsync(HttpContext.User);
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

            // Find all of the previous and upcoming tests for the instructor
            var instructorScheduledTests = _context.TestSchedules.Include(x => x.Test).Where(x => x.Test.applicationUser.Id == instructor.Id).ToList();
            var upcomingTests = _context.TestSchedules.Include(ts => ts.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.EndTime > DateTime.Now).Select(tts => tts.Test).Distinct().ToList();
            var previousTests = _context.TestSchedules.Include(ts => ts.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.EndTime < DateTime.Now).Select(tts => tts.Test).Distinct().ToList();

            // Find all of the StudentTestAssignments for the upcoming tests
            var upcomingStudentTestAssignments = _context.TestSchedules.Include(x => x.StudentTestAssignments).Where(x => upcomingTests.Any(y => y.TestId == x.TestId)).ToList();
            ViewBag.InProgress = upcomingStudentTestAssignments;

            // Find all unscheduled tests
            var instructorTests = _context.Tests.Include(x => x.TestSchedules).Where(x => x.InstructorId == instructor.Id).ToList();
            var unscheduledTests = instructorTests.Except(instructorScheduledTests.Select(x => x.Test)).ToList();
            foreach (Test t in unscheduledTests)
            {
                upcomingTests.Add(t);
            }

            // Grab the courses and departments
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();

            // Find any tests that need manual grading
            var manualGradingRequired = _context.StudentTestAssignments.Where(x => previousTests.Any(y => y.TestId == x.TestSchedule.TestId)).ToList();
            ViewBag.ManualGradingRequired = manualGradingRequired;

            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;
            ViewBag.UnscheduledTests = unscheduledTests;

            return View();
        }

        /// <summary>
        /// Allows the instructor to a view a course's section
        /// </summary>
        /// <param name="code">DepartmentCode + CourseCode</param>
        /// <param name="sectionNumber">Search criteria for the specific section number to view</param>
        /// <returns>ViewSection view</returns>
        [HttpGet, Route("Courses/{code}/{sectionNumber}")]
        public async Task<IActionResult> ViewSection(string code, int sectionNumber)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

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

            // Find all of the previous and upcoming tests for the instructor
            var instructorScheduledTests = _context.TestSchedules.Include(x => x.Test).Where(x => x.Test.applicationUser.Id == instructor.Id).ToList();
            var upcomingTests = _context.TestSchedules.Include(ts => ts.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.EndTime > DateTime.Now).Select(tts => tts.Test).Distinct().ToList();
            var previousTests = _context.TestSchedules.Include(ts => ts.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.EndTime < DateTime.Now).Select(tts => tts.Test).Distinct().ToList();

            // Find all of the StudentTestAssignments for the upcoming tests
            var upcomingStudentTestAssignments = _context.TestSchedules.Include(x => x.StudentTestAssignments).Where(x => upcomingTests.Any(y => y.TestId == x.TestId)).ToList();
            ViewBag.InProgress = upcomingStudentTestAssignments;

            // Find all unscheduled tests
            var instructorTests = _context.Tests.Include(x => x.TestSchedules).Where(x => x.InstructorId == instructor.Id).ToList();
            var unscheduledTests = instructorTests.Except(instructorScheduledTests.Select(x => x.Test)).ToList();
            foreach (Test t in unscheduledTests)
            {
                upcomingTests.Add(t);
            }

            // Grab the courses and departments
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();

            // Find any tests that need manual grading
            var manualGradingRequired = _context.StudentTestAssignments.Where(x => previousTests.Any(y => y.TestId == x.TestSchedule.TestId)).ToList();
            ViewBag.ManualGradingRequired = manualGradingRequired;

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

        #endregion Courses and Sections Actions

        #region Test Actions
        /// <summary>
        /// Allows the instructor to view all of his tests
        /// </summary>
        /// <returns>Tests View</returns>
        [HttpGet, Route("Tests")]
        public IActionResult Tests()
        {
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            // Find all of the previous and upcoming tests for the instructor
            var instructorScheduledTests = _context.TestSchedules.Include(x => x.Test).Where(x => x.Test.applicationUser.Id == instructor.Id).ToList();
            var upcomingTests = _context.TestSchedules.Include(ts => ts.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.EndTime > DateTime.Now && x.Test.InstructorId == instructor.Id).Select(tts => tts.Test).Distinct().ToList();
            var previousTests = _context.TestSchedules.Include(ts => ts.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.EndTime < DateTime.Now && x.Test.InstructorId == instructor.Id).Select(tts => tts.Test).Distinct().ToList();

            // Find all of the StudentTestAssignments for the upcoming tests
            var upcomingStudentTestAssignments = _context.TestSchedules.Include(x => x.StudentTestAssignments).Where(x => upcomingTests.Any(y => y.TestId == x.TestId)).ToList();
            ViewBag.InProgress = upcomingStudentTestAssignments;

            // Find all unscheduled tests
            var instructorTests = _context.Tests.Include(x => x.TestSchedules).Where(x => x.InstructorId == instructor.Id).ToList();
            var unscheduledTests = instructorTests.Except(instructorScheduledTests.Select(x => x.Test)).ToList();
            foreach(Test t in unscheduledTests)
            {
                upcomingTests.Add(t);
            }

            // Grab the courses and departments
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();

            // Find any tests that need manual grading
            var manualGradingRequired = _context.StudentTestAssignments.Where(x => previousTests.Any(y => y.TestId == x.TestSchedule.TestId)).ToList();
            ViewBag.ManualGradingRequired = manualGradingRequired;

            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;
            ViewBag.UnscheduledTests = unscheduledTests;
            return View();
        }

        /// <summary>
        /// Allows the instructor to create the first part of the test
        /// </summary>
        /// <returns>CreateTest View</returns>
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

        /// <summary>
        /// Saves the created test to the database
        /// </summary>
        /// <param name="test">Test object that contains information</param>
        /// <returns>Redirects to the EditTest page</returns>
        [HttpPost, Route("Tests/CreateTest")]
        public IActionResult CreateTest(Test test)
        {
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var course = _context.Courses.Find(test.CourseId);
            var department = _context.Departments.Find(course.DepartmentId);
            test.applicationUser = Instructor;
            test.IsVisible = false;
            test.URLSafeName = HttpUtility.HtmlEncode(test.TestName);

            _context.Tests.Add(test);
            _context.SaveChanges();

            return RedirectToAction("EditTest", "Instructor", new { DepartmentCode = department.DepartmentCode, CourseCode = course.CourseCode, URLSafeName = test.URLSafeName });
        }

        /// <summary>
        /// Allows the instructor to view a specific test
        /// </summary>
        /// <param name="DepartmentCode">string parameter that distinguishes which department the test belongs to</param>
        /// <param name="CourseCode">string parameter that distinguishes which course the test belongs to</param>
        /// <param name="URLSafeName">string url</param>
        /// <returns>ViewTest View</returns>
        [HttpGet, Route("Tests/View/{DepartmentCode}/{CourseCode}/{URLSafeName}")]
        public IActionResult ViewTest(string DepartmentCode, string CourseCode, string URLSafeName)
        {
            // Get Test info
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            var test = _context.Tests
                .Include(t => t.Course)
                    .ThenInclude(c => c.Department)
                .Include(t => t.TestSchedules)
                .Where(t => t.URLSafeName == URLSafeName && t.Course.CourseCode == CourseCode && t.InstructorId == Instructor.Id)
                .First();

            ViewBag.Test = test;

            // Grab the points for the test
            var param = new SqlParameter[] {
                    new SqlParameter() {
                        ParameterName = "@returnVal",
                        SqlDbType =  SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    },
                    new SqlParameter() {
                        ParameterName = "@inputTestId",
                        SqlDbType =  SqlDbType.BigInt,
                        Direction = ParameterDirection.Input,
                        Value = test.TestId
                    }};

            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetTotalTestPoints @inputTestId", param);
            if (Convert.IsDBNull(param[0].Value))
                ViewBag.TotalPoints = 0;
            else
                ViewBag.TotalPoints = (int)param[0].Value;

            // Grab the test sections for the test
            var testSections = _context.TestSections
                .Include(ts => ts.Questions)
                    .ThenInclude(x => x.MultipleChoiceAnswers)
                .Include(ts => ts.Questions)
                    .ThenInclude(x => x.MatchingQuestionSides)
                .Include(x => x.Questions)
                    .ThenInclude(x => x.MatchingAnswerSides)
                .Where(x => x.TestId == test.TestId)
                .ToList();

            int totalQuestions = 0;
            foreach (TestSection ts in testSections)
            {
                totalQuestions += ts.Questions.Count;
            }
            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.TestSections = testSections;

            // Grabs all multiple choice questions
            ViewBag.MultipleChoiceQuestions = _context.Questions
                .Include(mc => mc.TestSection)
                .Include(mc => mc.MultipleChoiceAnswers)
                .Where(mc => mc.TestSection.TestId == test.TestId)
                .ToList();

            return View();
        }

        /// <summary>
        /// Allows the instructor to review all the student's tests for a specific test
        /// </summary>
        /// <param name="DepartmentCode">string parameter that distinguishes which department the test belongs to</param>
        /// <param name="CourseCode">string parameter that distinguishes which course the test belongs to</param>
        /// <param name="URLSafeName">string url</param>
        /// <returns>ReviewTest View</returns>
        [HttpGet, Route("Tests/ReviewTest/View/{DepartmentCode}/{CourseCode}/{URLSafeName}")]
        public async Task<IActionResult> ReviewTest(string DepartmentCode, string CourseCode, string URLSafeName)
        {
            // Get Test info
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            var test = _context.Tests
                .Include(t => t.Course)
                    .ThenInclude(c => c.Department)
                .Include(t => t.TestSchedules)
                .Where(t => t.URLSafeName == URLSafeName && t.Course.CourseCode == CourseCode && t.InstructorId == Instructor.Id)
                .First();

            ViewBag.Test = test;

            var studentTestAssignments = _context.StudentTestAssignments.Where(x => x.TestSchedule.TestId == test.TestId ).ToList();
            List<ApplicationUser> students = new List<ApplicationUser>();
            foreach(StudentTestAssignment sta in studentTestAssignments)
            {
                ApplicationUser student = await _userManager.FindByIdAsync(sta.StudentId);
                students.Add(student);
            }

            ViewBag.Students = students;
         
            return View();
        }

        /// <summary>
        /// Allows the instructor to review the specified student's test
        /// </summary>
        /// <param name="DepartmentCode">string parameter that distinguishes which department the test belongs to</param>
        /// <param name="CourseCode">string parameter that distinguishes which course the test belongs to</param>
        /// <param name="URLSafeName">string url</param>
        /// <param name="StudentId">string parameter of the specific student</param>
        /// <returns></returns>
        [HttpGet, Route("Tests/ReviewStudentTest/{DepartmentCode}/{CourseCode}/{URLSafeName}/{StudentId}")]
        public async Task<IActionResult> ReviewStudentTest(string DepartmentCode, string CourseCode, string URLSafeName, string StudentId)
        {
            // Get Test info
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            var test = _context.Tests
                .Include(t => t.Course)
                    .ThenInclude(c => c.Department)
                .Include(t => t.TestSchedules)
                .Where(t => t.URLSafeName == URLSafeName && t.Course.CourseCode == CourseCode && t.InstructorId == Instructor.Id)
                .First();

            ViewBag.Test = test;

            // Grab the points for the test
            var param = new SqlParameter[] {
                    new SqlParameter() {
                        ParameterName = "@returnVal",
                        SqlDbType =  SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    },
                    new SqlParameter() {
                        ParameterName = "@inputTestId",
                        SqlDbType =  SqlDbType.BigInt,
                        Direction = ParameterDirection.Input,
                        Value = test.TestId
                    }};

            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetTotalTestPoints @inputTestId", param);
            if (Convert.IsDBNull(param[0].Value))
                ViewBag.TotalPoints = 0;
            else
                ViewBag.TotalPoints = (int)param[0].Value;

            // Grab the test sections for the test
            var testSections = _context.TestSections
                .Include(ts => ts.Questions)
                .Where(x => x.TestId == test.TestId)
                .ToList();

            int totalQuestions = 0;
            foreach (TestSection ts in testSections)
            {
                totalQuestions += ts.Questions.Count;
            }
            ViewBag.TotalQuestions = totalQuestions;
            ViewBag.TestSections = testSections;

            // Grabs all multiple choice questions
            ViewBag.MultipleChoiceQuestions = _context.Questions
                .Include(mc => mc.TestSection)
                .Include(mc => mc.MultipleChoiceAnswers)
                .Where(mc => mc.TestSection.TestId == test.TestId)
                .ToList();

            ViewBag.Student = await _userManager.FindByIdAsync(StudentId);

            return PartialView();
        }

        /// <summary>
        /// Allows the instructor to edit a test's information and add and remove sections
        /// </summary>
        /// <param name="DepartmentCode">string parameter that distinguishes which department the test belongs to</param>
        /// <param name="CourseCode">string parameter that distinguishes which course the test belongs to</param>
        /// <param name="URLSafeName">string url</param>
        /// <returns></returns>
        [HttpGet, Route("Tests/Edit/{DepartmentCode}/{CourseCode}/{URLSafeName}")]
        public IActionResult EditTest(string DepartmentCode, string CourseCode, string URLSafeName)
        {
            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var department = _context.Departments.Where(d => d.DepartmentCode == DepartmentCode).First();
            var course = _context.Courses.Where(c => c.DepartmentId == department.DepartmentId && c.CourseCode == CourseCode).First();
            var test = _context.Tests.Where(t => t.URLSafeName == URLSafeName && t.CourseId == course.CourseId).First();
            course.DepartmentCode = department.DepartmentCode;
            var sections = _context.Sections.Where(s => s.InstructorId == Instructor.Id && s.CourseId == course.CourseId).ToList();

            ViewBag.Test = test;
            ViewBag.Course = course;
            ViewBag.Sections = sections;

            return View();
        }

        // Allows the instructor to grade a test
        [HttpGet, Route("Tests/Grade/{DepartmentCode}/{CourseCode}/{URLSafeName}")]
        public IActionResult GradeTest(string DepartmentCode, string CourseCode, string URLSafeName)
        {

            var Instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var test = _context.Tests
                .Include(t => t.Course)
                    .ThenInclude(c => c.Department)
                .Where(t => t.URLSafeName == URLSafeName &&
                            t.Course.CourseCode == CourseCode &&
                            t.Course.Department.DepartmentCode == DepartmentCode)
                .First();

            // Fetch all students that took this test
            var studentTestAssignments = _context.StudentTestAssignments
                .Include(sta => sta.TestSchedule)
                .Include(sta => sta.ApplicationUser)
                .Where(sta => sta.TestSchedule.TestId == test.TestId)
                .ToList();

            // Fetch all questions that need to graded
            var questions = _context.Essays
                .Include(e => e.TestSection)
                .Where(e => e.TestSection.TestId == test.TestId)
                .ToList();

            // Fetch all student answers for this test
            var essayAnswers = _context.StudentEssayAnswers
                .Include(sea => sea.TestSchedule)
                .Where(sea => sea.TestSchedule.TestId == test.TestId)
                .ToList();

            // create the JObject to be passed to the front End
            JArray jEssayQuestions = new JArray();
            var questionNumber = 1;
            foreach (var essayQuestion in questions)
            {
                dynamic jQuestion = essayQuestion.ToJObject();
                jQuestion.selected = false;
                jQuestion.questionNumber = questionNumber++;

                JArray jStudentAnswers = new JArray();
                foreach (var student in studentTestAssignments.Select(sta => sta.ApplicationUser).ToList())
                {
                   
                   dynamic jStudentAnswer = new JObject();
                   var studentAnswer = essayAnswers.Where(sa => sa.StudentId == student.Id && sa.QuestionId == essayQuestion.QuestionId).FirstOrDefault();
                    
                   jStudentAnswer.studentId = student.Id;
                   jStudentAnswer.studentFullName = student.FullName;

                   if (studentAnswer == null)
                   {
                       jStudentAnswer.answered = false;
                   }
                   else
                   {
                       jStudentAnswer.answered = true;
                       jStudentAnswer.answer = studentAnswer.EssayAnswerGiven;
                       jStudentAnswer.pointsEarned = studentAnswer.PointsEarned;
                       jStudentAnswer.comment = studentAnswer.Comments;
                       jStudentAnswer.answerId = studentAnswer.AnswerId;

                   }

                   jStudentAnswers.Add(jStudentAnswer);
                   
                }
                jQuestion.studentAnswers = jStudentAnswers;

                jEssayQuestions.Add(jQuestion);

            }

            string strTest = JsonConvert.SerializeObject(jEssayQuestions);
            ViewBag.Test = test;
            ViewBag.Assigments = studentTestAssignments;
            ViewBag.EssayQuestions = strTest;

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
