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
using Microsoft.Extensions.Logging;
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

        // TODO Lacy - Create controller for the matching question

        #region get different question types
        /// <summary>
        /// Bima says that this method gets a true false questions, saves it to the DB, and returns the question
        /// </summary>
        /// <param name="jsonData">The object that contains all of the question information</param>
        /// <returns>The question that was created</returns>        
        [HttpPost, Route("NewTrueFalseQuestion")]
        public JsonResult NewTrueFalseQuestion([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            long testSectionId = json.TestSectionId;
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not alowed" });
            }
            else
            {
                // Create a new question and add it to the DB
                TrueFalse TFQuestion = new TrueFalse();
                TFQuestion.Content = json.Content;
                //TFQuestion.Index = int.Parse((string)json.Index);
                TFQuestion.PointValue = int.Parse((string)json.PointValue);
                TFQuestion.TestSectionId = testSectionId;
                TFQuestion.TrueFalseAnswer = bool.Parse((string)json.TrueFalseAnswer);
                TFQuestion.QuestionType = QuestionType.TrueFalse;

                _context.TrueFalses.Add(TFQuestion);
                _context.SaveChanges();

                return Json(new { success = true, question = TFQuestion });
            }
            
        }

        [HttpPost, Route("Matching")]
        public IActionResult NewMatching([FromBody] JObject jsonData)
        {

            return Json(new { success = true });
        }

        /// <summary>
        /// Bima says that this method gets a multiple choice question, saves it to the DB, and returns the question
        /// </summary>
        /// <param name="jsonData">The object that contains all of the question information</param>
        /// <returns>The question that was created</returns>
        [HttpPost, Route("MultipleChoice")]
        public JsonResult NewMultipleChoice([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            long testSectionId = long.Parse(json.TestSectionId);
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();
            JArray MCAnswers = json.MultipleChoiceAnswers;

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not alowed" });
            }
            else
            {
                Question MCQuestion = new Question();
                MCQuestion.Content = json.Content;
                MCQuestion.Index = int.Parse((string)json.Index);
                MCQuestion.PointValue = int.Parse((string)json.PointValue);
                MCQuestion.TestSectionId = long.Parse((string)json.TestSectionId);
                MCQuestion.QuestionType = QuestionType.MultipleChoice;

                List<MultipleChoiceAnswer> MCAList = new List<MultipleChoiceAnswer>();

                foreach (JObject x in MCAnswers)
                {
                    dynamic mca = x;
                    MCAList.Add(new MultipleChoiceAnswer()
                    {
                        MultipleChoiceAnswerOption = mca.MultipleChoiceAnswerOption,
                        IsCorrect = bool.Parse(mca.IsCorrect)
                    });
                }

                MCQuestion.MultipleChoiceAnswers = MCAList;
                _context.Questions.Add(MCQuestion);
                _context.SaveChanges();
                return Json(new { success = true, question = MCQuestion });
            }
        }

        /// <summary>
        /// Bima says that this method gets a Fill In The Blank question, saves it to the DB, and returns the question
        /// </summary>
        /// <param name="jsonData">The object that contains all of the essay question information</param>
        /// <returns>The fill in the blank question that was created</returns>
        [HttpPost, Route("Essay")]
        public JsonResult NewEssay([FromBody] JObject jsonData)
        {

            dynamic json = jsonData;
            long testSectionId = long.Parse(json.TestSectionId);
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not alowed" });
            }
            else
            { 
                Essay EssayQuestion = new Essay();
                EssayQuestion.Content = json.Content;
                EssayQuestion.Index = int.Parse((string)json.Index);
                EssayQuestion.PointValue = int.Parse((string)json.PointValue);
                EssayQuestion.TestSectionId = long.Parse((string)json.TestSectionId);
                EssayQuestion.QuestionType = QuestionType.Essay;
                EssayQuestion.ExpectedEssayAnswer = json.ExpectedEssayAnswer;

                _context.Questions.Add(EssayQuestion);
                _context.SaveChanges();
                return Json(new { success = true, question = EssayQuestion });
            }
        }

        /// <summary>
        /// Bima says that this method gets a Fill in the Blank questions, saves it to the DB, and returns the question
        /// </summary>
        /// <param name="jsonData">The object that contains all of the fill in the blank question information</param>
        /// <returns>The fill in the blank question that was created</returns>
        [HttpPost, Route("FillInTheBlank")]
        public JsonResult NewFillInTheBlank([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            long testSectionId = long.Parse(json.TestSectionId);
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();
            JArray getAnswers = json.FillInTheBlankAnswer;
            String answers = String.Empty;

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not alowed" });
            }
            else
            {
                FillInTheBlank FITBQuestion = new FillInTheBlank();
                FITBQuestion.Content = json.Content;
                FITBQuestion.Index = int.Parse((string)json.Index);
                FITBQuestion.PointValue = int.Parse((string)json.PointValue);
                FITBQuestion.TestSectionId = long.Parse((string)json.TestSectionId);
                FITBQuestion.QuestionType = QuestionType.FillInTheBlank;

                foreach(JObject x in answers)
                {
                    answers += x.ToString() + " ";
                }

                _context.Questions.Add(FITBQuestion);
                _context.SaveChanges();
                return Json(new { success = true, question = FITBQuestion });
            }
        }
        #endregion get different question types

        ///Bima says: receiving TestSectionId, delete it return true or false
        [HttpPost, Route("DeleteSectionTestId")]
        public JsonResult DeleteSectionTestId([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            long testSectionId = long.Parse(json.SectionTestId);
            long testId = long.Parse(json.TestId);

            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not alowed" });
            }
            
            if (testSection != null)
            {
                _context.TestSections.Remove(testSection);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
            
        }

        /// <summary>
        /// Bima says that this method gets all of the true false questions in a section
        /// </summary>
        /// <param name="testSectionId">Specifies which questions need to be grabbed</param>
        /// <returns>All of the questions in that test section</returns>

        ///Update section instruction - receive a section id, instrucions, and update the system
        ///return success true or false
        [HttpPost, Route("UpdateSectionInstruction")]
        public JsonResult UpdateSectionInstruction([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            long testSectionId = json.TestSectionId;
            string sectionInfo = json.SectionInstructions;

            // Verify that the user logged in matches the instructor's id on the testId on the sectionId
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not alowed" });
            }
            
            if (testSection != null)
            {
                testSection.SectionInstructions = sectionInfo;
                _context.TestSections.Update(testSection);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

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
            string sectionHeader = string.Empty;
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
                    NewSection.SectionInstructions = DefaultTestSectionText.Instruction.TrueFalse;
                    sectionHeader = DefaultTestSectionText.Header.TrueFalse;
                    break;
                case QuestionType.Matching:
                    NewSection.SectionInstructions = DefaultTestSectionText.Instruction.Matching;
                    sectionHeader = DefaultTestSectionText.Header.Matching;
                    break;
                case QuestionType.FillInTheBlank:
                    NewSection.SectionInstructions = DefaultTestSectionText.Instruction.FillInTheBlank;
                    sectionHeader = DefaultTestSectionText.Header.FillInTheBlank;
                    break;
                case QuestionType.MultipleChoice:
                    NewSection.SectionInstructions = DefaultTestSectionText.Instruction.MultipleChoice;
                    sectionHeader = DefaultTestSectionText.Header.MultipleChoice;
                    break;
                case QuestionType.Essay:
                    NewSection.SectionInstructions = DefaultTestSectionText.Instruction.Essay;
                    sectionHeader = DefaultTestSectionText.Header.Essay;
                    break;
            }
            
            test.TestSections.Add(NewSection);
            _context.SaveChanges();


            return Json(new { success = true, sectionId = NewSection.TestSectionId, instructions = NewSection.SectionInstructions, sectionType = NewSection.QuestionType, header = sectionHeader});
        }

        /// <summary>
        /// Bima says he needs error checking: 1) if the assigned test date is passed assign error message, 
        /// 2) If the test time limit is negative or 0, assign error message
        /// 3) if no sections or students assigned, return error message
        /// 4) INdividual students cannot be assigned 
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        [HttpPost, Route("NewTestSchedule")]
        public JsonResult NewTestSchedule([FromBody]JObject jsonData)
        {
            // Receiveing the data
            dynamic json = jsonData;
            long testId = json.TestId;
            string startEndDateTime = json.StartEndDateTime;
            DateTime testDate = Convert.ToDateTime(startEndDateTime.Split(" ").First());
            bool isTimeUnlimited = bool.Parse((string) json.IsTimeUnlimited);
            int timeLimit = json.TimeLimit;
            JArray studentIds = json.Students;
            JArray sectionIds = json.Sections;

            // Error checking variables
            JObject errorMessages = new JObject();
            int errorCount = 0;

            if (testDate < DateTime.Now)
            {
                errorMessages.Add( new JProperty("DateTimeError", "The test date must be set to today's date or further."));
                errorCount++;
            }
            if (timeLimit <= 0)
            {
                errorMessages.Add(new JProperty("TimeLimitError", "The test time must be set and must be greater than 0. "));
                errorCount++;
            }
            if (studentIds.Count <= 0 && sectionIds.Count <= 0)
            {
                errorMessages.Add(new JProperty("StudentSectionErrorMessage", "The test must be assigned at least one section or at least one student" ));
                errorCount++;
            }
            if (errorCount > 0)
            {
                return Json(new { success = false, ErrorMessage = errorMessages });
            }

            // Parsing the date
            string startDateTime = startEndDateTime.Substring(0, startEndDateTime.IndexOf(" - "));
            string endDateTime = startEndDateTime.Substring(startEndDateTime.IndexOf(" - ") + 3);
            DateTime parsedStartDateTime = DateTime.ParseExact(startDateTime, "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime parsedEndDateTime   = DateTime.ParseExact(endDateTime,   "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);

            // Create the new Schedule
            var newTestSchedule = new TestSchedule()
            {
                StartTime = parsedStartDateTime,
                EndTime = parsedEndDateTime,
                TestId = testId,
                TimeLimit = isTimeUnlimited ? 0 : timeLimit,
                IsTimeUnlimited = isTimeUnlimited
            };

            // Assigns all the students in the section to that  
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

        // Get all the Test Schedules for a test
        [HttpGet, Route("GetTestSchedules/{testId}")]
        public JsonResult GetTestSchedules(long testId)
        {
            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            // Find the test that the instructor has selected
            var test = _context.Tests.Find(testId);

            // Find all of the test schedules associated with this test
            var testSchedules = _context.TestSchedules.Include(ts => ts.StudentTestAssignments).Where(ts => ts.TestId == testId).ToList();
            var sections = _context.Sections.Include(s => s.Enrollments).Where(s => s.InstructorId == instructor.Id && s.CourseId == test.CourseId).ToList();

            List<JObject> schedules = new List<JObject>();

            // Create and return the sections and students that are assigned to the test, and elminate the students who are not
            foreach (var testSchedule in testSchedules)
            {
                var StudentsAssignedIds = testSchedule.StudentTestAssignments.Select(sta => sta.StudentId).ToList();

                dynamic testScheduleJObject = new JObject();
                testScheduleJObject.Availability = string.Format("MM / dd / yyyy hh: mm tt", testSchedule.StartTime) + " - " + string.Format("MM / dd / yyyy hh: mm tt", testSchedule.EndTime);
                testScheduleJObject.TimeLimit = testSchedule.IsTimeUnlimited ? "Unlimited" : testSchedule.TimeLimit.ToString() + " minutes";

               // Checks if all students from a section were assigned
               List<int> entireSectionsAssigned = new List<int>();
                foreach (Section section in sections)
                {
                    var sectionStudentsIds = section.Enrollments.Select(e => e.StudentId).ToList();
                    
                    if(StudentsAssignedIds.Intersect(sectionStudentsIds).Count() == sectionStudentsIds.Count())
                    {
                        entireSectionsAssigned.Add(section.SectionNumber);
                        StudentsAssignedIds = StudentsAssignedIds.Except(sectionStudentsIds).ToList();
                    }
                }

                if (entireSectionsAssigned.Any())
                {
                    testScheduleJObject.Assigmnet += "Sections " + String.Join(", ", entireSectionsAssigned.ToArray());
                }

                schedules.Add(testScheduleJObject);
            }

            
            
            return Json(new {success = true, schedules });
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