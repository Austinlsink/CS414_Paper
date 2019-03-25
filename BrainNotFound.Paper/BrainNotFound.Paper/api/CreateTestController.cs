using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BrainNotFound.Paper.api
{
    [Route("api/CreateTest")]
    public class CreateTestController : Controller
    {
        #region Initialize controllers
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        /// 
        public CreateTestController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("CreateTestSection")]
        public JsonResult CreateTestSection(long TestId, string QuestionTypeName)
        {
            var test = _context.Tests.Find(TestId);
            var questionType = _context.QuestionTypes.Where(qt => qt.Name == QuestionTypeName).First();
            var NewSection = new TestSection()
            {
                QuestionType = questionType,
                IsQuestionSection = true,
                SectionInstructions = DefaultInstruction.TrueFalse
            };

            test.TestSections.Add(NewSection);
            _context.SaveChanges();

            return Json(NewSection);
        }

        // Creates a new Test Schedule
        [HttpPost, Route("NewTestSchedule")]
        public JsonResult NewTestSchedule([FromBody]JObject jsonData)
        {
            // Receiveing the data
            dynamic json = jsonData;
            long testId = json.TestId;
            string startEndDateTime = json.StartEndDateTime;
            bool isTimeUnlimited = bool.Parse((string) json.IsTimeUnlimited);
            int timeLimit = json.TimeLimit;
            List<string> studentIds = json.Students;
            List<long> sectionIds = json.Sections;


            var me = "";
            var test = _context.Tests.Find(testId);
            me += test.TestName + " / " + startEndDateTime;



            return Json(new { success = true, testName = test.TestName, startEndDateTime, isTimeUnlimited , timeLimit , studentIds , sectionIds });
        }

        // Gets all the students in a section
        [HttpGet, Route("GetStudentsInSection/{SectionId}")]
        public JsonResult GetSection(long SectionId)
        {

            var enrollments = _context.Enrollments.Include(e => e.ApplicationUser).Where(e => e.SectionId == SectionId).ToList();
            List<JObject> students = new List<JObject>();

            foreach(Enrollment enrollment in enrollments)
            {
                dynamic student = new JObject();
                student.FirstName = enrollment.ApplicationUser.FirstName;
                student.LastName = enrollment.ApplicationUser.LastName;
                student.Id = enrollment.ApplicationUser.Id;
                students.Add(student);
            }

            return Json(students);
        }       
    }
}