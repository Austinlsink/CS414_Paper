using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        // TODO Lacy - create controller actions for each type of question: TF, MC, Matching, FitB, Essay; QUESTION is my model

        /// <summary>
        /// Bima says that this method gets a true false questions, saves it to the DB, and returns the question
        /// </summary>
        /// <param name="jsonData">The object that contains all of the question information</param>
        /// <returns>The question that was created</returns>        
        [HttpPost, Route("TrueFalse")]
        public JsonResult GetTrueFalse([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;

            // Create a new question and add it to the DB
            TrueFalse TFQuestion = new TrueFalse();
            TFQuestion.Content = json.Content;
            TFQuestion.Index = int.Parse((string) json.Index);
            TFQuestion.PointValue = int.Parse((string) json.PointValue);
            TFQuestion.TestSectionId = long.Parse((string) json.TestSectionId);
            TFQuestion.TrueFalseAnswer = bool.Parse((string) json.TrueFalseAnswer);
            TFQuestion.QuestionType = QuestionType.TrueFalse;

            _context.Questions.Add(TFQuestion);
            _context.SaveChanges();         
            
            return Json(new { success = true, question = TFQuestion });
        }

        [HttpPost, Route("MultipleChoice")]
        public JsonResult GetMultipleChoice([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            JArray MCAnswers = json.MultipleChoiceAnswers;


            Question MCQuestion = new Question();
            MCQuestion.Content = json.Content;
            MCQuestion.Index = int.Parse((string)json.Index);
            MCQuestion.PointValue = int.Parse((string)json.PointValue);
            MCQuestion.TestSectionId = long.Parse((string)json.TestSectionId);
            MCQuestion.QuestionType = QuestionType.MultipleChoice;

            return Json(new { success = true });
        }

        /// <summary>
        /// Bima says that this method gets all of the true false questions in a section
        /// </summary>
        /// <param name="testSectionId">Specifies which questions need to be grabbed</param>
        /// <returns>All of the questions in that test section</returns>
        [HttpGet, Route("GetQuestionsInSection/{testSectionId}")]
        public JsonResult GetQuestionsInSection(long testSectionId)
        {
            var questions = _context.Questions.Where(q => q.TestSectionId == testSectionId).ToList();

            return Json(questions);
        }


        [HttpPost, Route("CreateTestSection")]
        public JsonResult CreateTestSection([FromBody]JObject jsonData)
        {
            // Converting the data from the json object to variables
            dynamic json = jsonData;
            long testId = json.TestId;
            string questionType = json.QuestionType;

            // TODO: Check if test really belongs to the current teacher

            // Find the test and create a section in it
            
            var test = _context.Tests.Find(testId);
            test.TestSections = new List<TestSection>();
            
            var NewSection = new TestSection()
            {
                QuestionType = questionType,
                IsQuestionSection = true,
            };

            // Sets the Instuction to the section
            switch(questionType)
            {
                case QuestionType.TrueFalse:
                    NewSection.SectionInstructions = DefaultInstruction.TrueFalse;
                    break;
            }
            
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
            JArray studentIds = json.Students;
            JArray sectionIds = json.Sections;


            // Parsing the date
            string startDateTime = startEndDateTime.Substring(0, startEndDateTime.IndexOf(" - "));
            string endDateTime = startEndDateTime.Substring(startEndDateTime.IndexOf(" - ") + 3);
            DateTime parsedStartDateTime = DateTime.ParseExact(startDateTime, "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime parsedEndDateTime   = DateTime.ParseExact(endDateTime,   "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);

            var newTestSchedule = new TestSchedule()
            {
                StartTime = parsedStartDateTime,
                EndTime = parsedEndDateTime,
                TestId = testId,
                TimeLimit = isTimeUnlimited ? 0 : timeLimit,
                IsTimeUnlimited = isTimeUnlimited
            };

            newTestSchedule.StudentTestAssignments = new List<StudentTestAssignment>();

            // Get all students in sections
            foreach (long sectionId in sectionIds)
            {
                var section = _context.Sections.Include(s => s.Enrollments).Where(s => s.SectionId == sectionId).First();
                foreach (Enrollment en in section.Enrollments)
                {
                    newTestSchedule.StudentTestAssignments.Add(new StudentTestAssignment()
                    {
                        StudentId = en.StudentId
                    });
                }
            }

            _context.TestSchedules.Add(newTestSchedule);
            _context.SaveChanges();

            return Json(new { success = true });
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