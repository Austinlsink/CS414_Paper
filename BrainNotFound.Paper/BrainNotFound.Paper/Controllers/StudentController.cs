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
using BrainNotFound.Paper.Services;
using System.Data.SqlClient;
using System.Data;

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

        #endregion Student profile and Settings controllers

        /// <summary>
        /// Displays the student's upcoming and previous tests
        /// </summary>
        /// <returns>Index View</returns>
        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public IActionResult Index()
        {
            // Find the student information and his test assignments
            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var studentTestAssignments = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).Where(x => x.StudentId == student.Id).ToList();

            // Distinguish between upcoming tests and previous tests
            var upcomingTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime > DateTime.Now && sta.Submitted == false).Select(sta => sta.TestSchedule).ToList();
            var previousTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime < DateTime.Now || sta.Submitted == true).Select(sta => sta.TestSchedule).ToList();
            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;

            // Grab each tests course and department information
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();
            ViewBag.Courses = courses;
            ViewBag.Departments = departments;
            return View();
        }

        /// <summary>
        /// Allows the user to view all of his instructors and their contact information
        /// </summary>
        /// <returns>Instructors View</returns>
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
                if (!(instructors.Where(i => i.Id == s.InstructorId).Any()))
                    instructors.Add(await _userManager.FindByIdAsync(s.InstructorId));
            }

            // Find the specified user by his username and add him to the ViewBag
            ViewBag.instructorList = instructors;

            return View();
        }

        /// <summary>
        /// Allows the student to view a specific section and its information
        /// </summary>
        /// <param name="code">DepartmentCode + CourseCode</param>
        /// <param name="sectionNumber">section number for the corresonding course</param>
        /// <returns>Section View</returns>
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

            // Distinguish the tests as either upcoming or previous
            var upcomingTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime > DateTime.Now).Select(sta => sta.TestSchedule.Test).Where(sta => sta.CourseId == course.CourseId).ToList();
            var previousTests = studentTestAssignments.Where(sta => sta.TestSchedule.EndTime < DateTime.Now).Select(sta => sta.TestSchedule.Test).Where(sta => sta.CourseId == course.CourseId).ToList();
            ViewBag.UpcomingTests = upcomingTests;
            ViewBag.PreviousTests = previousTests;

            return View();
        }

        #region Test Controllers

        /// <summary>
        /// Allows the student to take the specified tests, and grabs any questions that may already be answered
        /// </summary>
        /// <param name="testScheduleId">Search criteria for the specified test</param>
        /// <returns>TakeTest View</returns>
        [HttpGet, Route("Tests/TakeTest/{testScheduleId}")]
        public async Task<IActionResult> TakeTest(long testScheduleId)
        {
            // Find the student's information and his test assignment
            ApplicationUser student = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Student = student;
            var studentTestAssignment = _context.StudentTestAssignments
                                .Include(x => x.TestSchedule)
                                    .ThenInclude(x => x.Test)
                                        .ThenInclude(x => x.Course)
                                            .ThenInclude(x => x.Department)
                                .Where(x => x.Submitted == false && x.TestScheduleId == testScheduleId).First();

            // Grab the test's information
            var testInformation = studentTestAssignment.TestSchedule.Test;
            ViewBag.TestInformation = testInformation;

            // Grab the testschedule and its ID to pass as a hidden parameter to the ajax call
            ViewBag.TestSchedule = studentTestAssignment.TestSchedule;

            //Set the student's start time
            if (studentTestAssignment.StartedTime.Equals("0001-01-01 00:00:00.0000000"))
            {
                studentTestAssignment.StartedTime = DateTime.Now;
            }

            // Set the time limit for the test
            DateTime dateTime = new DateTime();
            if (studentTestAssignment.TestSchedule.IsTimeUnlimited)
            {
                dateTime = studentTestAssignment.TestSchedule.EndTime;
            }
            else
            {
                dateTime = studentTestAssignment.StartedTime.AddMinutes(studentTestAssignment.TestSchedule.TimeLimit);
            }
            ViewBag.TimeLimit = dateTime;

            // Grab the test sections for the test
            var testSections = _context.TestSections
                .Include(ts => ts.Questions)
                    .ThenInclude(q => q.MultipleChoiceAnswers)
                .Include(ts => ts.Questions)
                    .ThenInclude(x => x.MatchingQuestionSides)
                .Include(ts => ts.Questions)
                    .ThenInclude(x => x.MatchingAnswerSides)
                .Where(x => x.TestId == studentTestAssignment.TestSchedule.TestId)
                .ToList();

            // Grab the student answers for the test if any
            var studentAnswers = _context.StudentAnswers
                .Include(sa => sa.StudentMultipleChoiceAnswers)
                .Where(sa => sa.StudentId == student.Id && sa.TestScheduleId == studentTestAssignment.TestScheduleId).ToList();

            // Test information variables
            int totalQuestions = 0;

            // Fetching all Questions for test
            for (int j = 0; j < testSections.Count; j++)
            {
                for (int i = 0; i < testSections[j].Questions.Count; i++)
                {
                    switch (testSections[j].QuestionType)
                    {
                        case QuestionType.TrueFalse:
                            var studentTFAnswer = _context.StudentTrueFalseAnswers
                                .Where(stfa => stfa.QuestionId == testSections[j].Questions[i].QuestionId && stfa.TestScheduleId == studentTestAssignment.TestScheduleId && stfa.StudentId == student.Id)
                                .FirstOrDefault();

                            if (studentTFAnswer == null)
                            {
                                testSections[j].Questions[i].studentTrueFalseAnswer = null;
                            }
                            else
                            {
                                testSections[j].Questions[i].studentTrueFalseAnswer = studentTFAnswer.TrueFalseAnswerGiven;
                            }
                            totalQuestions += 1;
                            break;
                            
                        case QuestionType.MultipleChoice:
                            var studentMCAnswers = _context.StudentAnswers.Include(sa => sa.StudentMultipleChoiceAnswers).Where(sa => sa.QuestionId == testSections[j].Questions[i].QuestionId && sa.TestScheduleId == studentTestAssignment.TestScheduleId && sa.StudentId == student.Id).FirstOrDefault();

                            if (studentMCAnswers == null)
                            {
                                testSections[j].Questions[i].studentMultipleChoiceAnswers = new List<StudentMultipleChoiceAnswer>();
                            }
                            else
                            {
                                testSections[j].Questions[i].studentMultipleChoiceAnswers = studentMCAnswers.StudentMultipleChoiceAnswers;
                            }
                            totalQuestions += 1;
                            break;
                        case QuestionType.Essay:
                            var studentEssayAnswer = _context.StudentEssayAnswers.Where(sea => sea.QuestionId == testSections[j].Questions[i].QuestionId && sea.TestScheduleId == studentTestAssignment.TestScheduleId && sea.StudentId == student.Id).FirstOrDefault();

                            if (studentEssayAnswer == null)
                            {
                                testSections[j].Questions[i].studentEssayAnswer = null;
                            }
                            else
                            {
                                testSections[j].Questions[i].studentEssayAnswer = studentEssayAnswer.EssayAnswerGiven;
                            }
                            totalQuestions += 1;
                            break;
                        case QuestionType.Matching:
                            var studentMatchingAnswer = _context.StudentMatchingAnswers
                                                            .Include(x => x.StudentAnswer)
                                                            .Where(x => x.StudentAnswer.QuestionId == testSections[j].Questions[i].QuestionId && x.StudentAnswer.StudentId == student.Id).ToList();

                            if (studentMatchingAnswer == null)
                            {
                                testSections[j].Questions[i].studentMatchingAnswers = new List<StudentMatchingAnswer>(); ;
                            }
                            else
                            {
                                testSections[j].Questions[i].studentMatchingAnswers = studentMatchingAnswer;
                            }
                            totalQuestions += 1;
                            break;
                    }
                }
            }

            // Get the total points for the test
            var totalPoints = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).Where(x => x.StudentId == student.Id && x.Submitted == false && x.TestScheduleId == testScheduleId).First();

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
                        Value = totalPoints.TestSchedule.TestId
                    }};

            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetTotalTestPoints @inputTestId", param);

            if (Convert.IsDBNull(param[0].Value))
                totalPoints.totalPoints = 0;
            else
                totalPoints.totalPoints = (int)param[0].Value;
           
            _context.SaveChanges();

            ViewBag.TotalPoints = totalPoints;
            ViewBag.TestSections = testSections;
            ViewBag.TotalQuestions = totalQuestions;

            return View();
        }

        /// <summary>
        /// Allows the student to review his test after it has been taken
        /// </summary>
        /// <param name="testScheduleId">Search criteria for the specific test</param>
        /// <returnsReviewTest View></returns>
        [HttpGet, Route("Tests/ReviewTest/{testScheduleId}")]
        public async Task<IActionResult> ReviewTest(long testScheduleId)
        {
            // Find the student information and his test assignments
            ApplicationUser student = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Student = student;
            var studentTestAssignment = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.Submitted == true && x.TestScheduleId == testScheduleId).First();
            ViewBag.StudentTestAssignment = studentTestAssignment;

            // Grab the test's information
            var testInformation = studentTestAssignment.TestSchedule.Test;
            ViewBag.TestInformation = testInformation;
            ViewBag.TestSchedule = studentTestAssignment.TestSchedule; // Grab the testschedule and its ID to pass as a hidden parameter to the ajax call

            // Grab the test sections for the test
            var testSections = _context.TestSections.Include(ts => ts.Questions).ThenInclude(q => q.MultipleChoiceAnswers).Where(x => x.TestId == studentTestAssignment.TestSchedule.TestId).ToList();
            ViewBag.TestSections = testSections;

            var studentAnswers = _context.StudentAnswers
                .Include(sa => sa.StudentMultipleChoiceAnswers)
                .Where(sa => sa.StudentId == student.Id && sa.TestScheduleId == studentTestAssignment.TestScheduleId).ToList();
            

            // Test information variables
            int totalQuestions = 0;

            // Fetching all Questions for test
            for (int j = 0; j < testSections.Count; j++)
            {
                for (int i = 0; i < testSections[j].Questions.Count; i++)
                {
                    switch (testSections[j].QuestionType)
                    {
                        case QuestionType.TrueFalse:
                            var studentTFAnswer = _context.StudentTrueFalseAnswers
                                .Where(stfa => stfa.QuestionId == testSections[j].Questions[i].QuestionId && stfa.TestScheduleId == studentTestAssignment.TestScheduleId)
                                .FirstOrDefault();

                            if (studentTFAnswer == null)
                            {
                                testSections[j].Questions[i].studentTrueFalseAnswer = null;
                            }
                            else
                            {
                                testSections[j].Questions[i].studentTrueFalseAnswer = studentTFAnswer.TrueFalseAnswerGiven;

                            }
                            totalQuestions += 1;
                            break;

                        case QuestionType.MultipleChoice:
                            var studentMCAnswers = _context.StudentAnswers.Include(sa => sa.StudentMultipleChoiceAnswers).Where(sa => sa.QuestionId == testSections[j].Questions[i].QuestionId && sa.TestScheduleId == studentTestAssignment.TestScheduleId).FirstOrDefault();

                            if (studentMCAnswers == null)
                            {
                                testSections[j].Questions[i].studentMultipleChoiceAnswers = new List<StudentMultipleChoiceAnswer>();
                            }
                            else
                            {
                                testSections[j].Questions[i].studentMultipleChoiceAnswers = studentMCAnswers.StudentMultipleChoiceAnswers;

                            }
                            totalQuestions += 1;
                            break;
                        case QuestionType.Essay:
                            var studentEssayAnswer = _context.StudentEssayAnswers.Where(sea => sea.QuestionId == testSections[j].Questions[i].QuestionId && sea.TestScheduleId == studentTestAssignment.TestScheduleId).FirstOrDefault();

                            if (studentEssayAnswer == null)
                            {
                                testSections[j].Questions[i].studentEssayAnswer = null;
                            }
                            else
                            {
                                testSections[j].Questions[i].studentEssayAnswer = studentEssayAnswer.EssayAnswerGiven;
                            }
                            totalQuestions += 1;
                            break;
                    }
                }
            }
            ViewBag.TotalQuestions = totalQuestions;

            // Get the student's grades
            var grades = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).Where(x => x.StudentId == student.Id && x.Submitted == true && x.TestScheduleId == testScheduleId).First();

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
                        Value = grades.TestSchedule.TestId
                    }};

            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetTotalTestPoints @inputTestId", param);

            if (Convert.IsDBNull(param[0].Value))
                grades.totalPoints = 0;
            else
                grades.totalPoints = (int)param[0].Value;

            ViewBag.Grades = grades;
            
            return View();
        }
        #endregion Test Controllers

        /// <summary>
        /// Allows the student to view all of his grades for each class
        /// </summary>
        /// <returns>Grades View</returns>
        [HttpGet, Route("Grades")]
        public async Task<IActionResult> Grades()
        {
            // Get the courses that the student is enrolled in
            ApplicationUser student = await _userManager.FindByNameAsync(User.Identity.Name);
            var enrollments = _context.Enrollments.Include(x => x.Section).ThenInclude(x => x.Course).ThenInclude(x => x.Department).Where(x => x.StudentId == student.Id).ToList();
            ViewBag.Enrollments = enrollments;

            // Get the student's grades
            var grades = _context.StudentTestAssignments.Include(x => x.TestSchedule).ThenInclude(x => x.Test).ThenInclude(x => x.Course).Where(x => x.StudentId == student.Id && x.Submitted == true).OrderBy(x => x.TestSchedule.EndTime).ToList();

            for(int i = 0; i < grades.Count; i++)
            {
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
                            Value = grades[i].TestSchedule.TestId
                        }};

                _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetTotalTestPoints @inputTestId", param);

                if (Convert.IsDBNull(param[0].Value))
                    grades[i].totalPoints = 0;
                else
                    grades[i].totalPoints = (int) param[0].Value;
            }

            ViewBag.Grades = grades;

            return View();
        }

        /// <summary>
        /// Displays an error message if something is amiss
        /// </summary>
        /// <returns>Error View</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}