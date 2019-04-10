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
using Microsoft.EntityFrameworkCore;

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

            // Find all of the tests for this course
            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var studentTestAssignments = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).Where(x => x.StudentId == student.Id).ToList();

            var upcomingTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime > DateTime.Now).Select(sta => sta.TestSchedule.Test).Where(sta => sta.CourseId == course.CourseId).ToList();
            var previousTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime < DateTime.Now).Select(sta => sta.TestSchedule.Test).Where(sta => sta.CourseId == course.CourseId).ToList();

            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;

            return View();
        }

        #region Test Controllers
        [HttpGet, Route("Tests")]
        public IActionResult Tests()
        {
            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var studentTestAssignments = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).Where(x => x.StudentId == student.Id).ToList();

            var upcomingTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime > DateTime.Now).Select(sta => sta.TestSchedule).ToList();
            var previousTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime < DateTime.Now).Select(sta => sta.TestSchedule).ToList();

            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();
            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;
            ViewBag.Courses = courses;
            ViewBag.Departments = departments;
            return View();
        }

        [HttpGet, Route("Tests/TestSummary")]
        public IActionResult TestSummary()
        {
            return View();
        }

        /// <summary>
        /// Allows the user to take the specified tests, and grabs any questions that may already be answered
        /// </summary>
        /// <param name="testScheduleId"></param>
        /// <returns></returns>
        [HttpGet, Route("Tests/TakeTest/{testScheduleId}")]
        public async Task<IActionResult> TakeTest(long testScheduleId)
        {
            // Student information
            ApplicationUser student = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Student = student;
        
            var testSchedule = _context.TestSchedules
                .Include(ts => ts.Test)
                    .ThenInclude(t => t.Course)
                        .ThenInclude(c => c.Department)
                .Where(ts => ts.TestScheduleId == testScheduleId)
                .First();

            // Grab the test information
            var testInformation = testSchedule.Test;
            ViewBag.TestInformation = testInformation;

            // Grab the testschedule and its ID to pass as a hidden parameter to the ajax call
            ViewBag.TestSchedule = testSchedule;

            // Grab the test sections for the test
            var testSections = _context.TestSections.Include(ts =>  ts.Questions).Where(x => x.TestId == testSchedule.TestId).ToList();
            ViewBag.TestSections = testSections;

            var allTFQuestions = _context.TrueFalses.ToList();
            List<TrueFalse> TFQuestions = new List<TrueFalse>();
            var allMCQuestions = _context.Questions.Include(x => x.MultipleChoiceAnswers).Where(x => x.QuestionType == "MultipleChoice");
            List<Question> MCQuestions = new List<Question>();
            var allMCOptions = _context.MultipleChoiceAnswers.ToList();
            List<MultipleChoiceAnswer> MCOptions = new List<MultipleChoiceAnswer>();
            int totalPoints = 0;
            // Grab the questions for each test section
            foreach(TestSection ts in testSections)
            {
                foreach(TrueFalse tf in allTFQuestions)
                {
                    if (tf.TestSectionId == ts.TestSectionId)
                    {
                        TFQuestions.Add(tf);
                        totalPoints += tf.PointValue;
                    }
                }
                foreach(Question q in allMCQuestions)
                {
                    if (ts.TestSectionId == q.TestSectionId)
                    {
                        foreach(MultipleChoiceAnswer option in allMCOptions)
                        {
                            if (q.QuestionId == option.QuestionId)
                            {
                                MCOptions.Add(option);
                            }
                        }
                        q.MultipleChoiceAnswers = MCOptions;
                        MCQuestions.Add(q);
                        totalPoints += q.PointValue;
                    }
                }
            }

            // Add the test section's questions to their bags
            ViewBag.TFQuestions = TFQuestions;
            ViewBag.MCQuestions = MCQuestions;

            // Add the total points to the bag
            ViewBag.TotalPoints = totalPoints;

            // Grab student's True false answers if any
            var studentTFAnswers = _context.StudentTrueFalseAnswers.Where(x => x.TestScheduleId == testSchedule.TestScheduleId).ToList();
            ViewBag.StudentTFAnswers = studentTFAnswers;

            // Grab the student's multiple choice answers if any
            var studentMCAnswers = _context.StudentMultipleChoiceAnswers.Include(x => x.StudentAnswer).Where(x => x.StudentAnswer.TestScheduleId == testSchedule.TestScheduleId).ToList();
            ViewBag.StudentMCAnswers = studentMCAnswers;


            return View();
        }

        [HttpGet, Route("Tests/ReviewTest")]
        public IActionResult ReviewTest()
        {
            return View();
        }
        #endregion Test Controllers

        [HttpGet, Route("Grades")]
        public async Task<IActionResult> Grades()
        {
            // Get the courses that the student is enrolled in
            ApplicationUser student = await _userManager.FindByNameAsync(User.Identity.Name);
            var enrollments = _context.Enrollments.Include(x => x.Section).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.StudentId == student.Id).ToList();
            ViewBag.Enrollments = enrollments;

            // Get the student's grades
            var grades = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).ThenInclude(x => x.Course).Where(x => x.StudentId == student.Id).ToList();
            ViewBag.Grades = grades;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
