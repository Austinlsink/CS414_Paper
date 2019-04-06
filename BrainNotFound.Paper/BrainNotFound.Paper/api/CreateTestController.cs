using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            long testSectionId = json.testSectionId;
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();
            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not allowed" });
            }
            else
            {
                // Create a new question and add it to the DB
                TrueFalse TFQuestion = new TrueFalse
                {
                    Content = json.content,
                    // Index = int.Parse((string)json.Index);
                    PointValue = json.pointValue,
                    TestSectionId = testSectionId,
                    TrueFalseAnswer = bool.Parse((string)json.answer),
                    QuestionType = QuestionType.TrueFalse
                };

                _context.TrueFalses.Add(TFQuestion);
                _context.SaveChanges();

                // Create the response Object
                dynamic question = new JObject();
                question.pointValue = TFQuestion.PointValue;
                question.content = TFQuestion.Content;
                question.answer = TFQuestion.TrueFalseAnswer;
                question.questionId = TFQuestion.QuestionId;

                return Json(new { success = true, question });
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
        [HttpPost, Route("NewMultipleChoiceQuestion")]
        public JsonResult NewMultipleChoiceQuestion([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            long testSectionId = json.testSectionId;
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();
            JArray MCAnswers = json.multipleChoiceAnswers;

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not allowed" });
            }
            else
            {
                Question MCQuestion = new Question
                {
                    Content = json.questionContent,
                    //Index = int.Parse((string)json.Index),
                    PointValue = json.pointValue,
                    TestSectionId = testSectionId,
                    QuestionType = QuestionType.MultipleChoice
                };

                List<MultipleChoiceAnswer> MCAList = new List<MultipleChoiceAnswer>();

                foreach (JObject x in MCAnswers)
                {
                    dynamic mca = x;
                    MCAList.Add(new MultipleChoiceAnswer()
                    {
                        MultipleChoiceAnswerOption = mca.optionContent,
                        IsCorrect = mca.isCorrect
                    });
                }

                MCQuestion.MultipleChoiceAnswers = MCAList;
                _context.Questions.Add(MCQuestion);
                _context.SaveChanges();


                // Create the response Object
                dynamic question = new JObject();
                question.pointValue = MCQuestion.PointValue;
                question.content = MCQuestion.Content;
                question.questionId = MCQuestion.QuestionId;
                question.sectionId = MCQuestion.TestSectionId;

                JArray MCOptions = new JArray();
                foreach(var option in MCQuestion.MultipleChoiceAnswers)
                {
                    dynamic MCOption = new JObject();
                    MCOption.multipleChoiceAnswerId = option.MultipleChoiceAnswerId;
                    MCOption.isCorrect = option.IsCorrect;
                    MCOption.optionContent = option.MultipleChoiceAnswerOption;

                    MCOptions.Add(MCOption);
                }
                question.multipleChoiceAnswers = MCOptions;

                return Json(new { success = true, question});
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
                return Json(new { success = false, error = "Instructor not allowed" });
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
        //[HttpPost, Route("FillInTheBlank")]
        //public JsonResult NewFillInTheBlank([FromBody] JObject jsonData)
        //{
        //    dynamic json = jsonData;
        //    long testSectionId = long.Parse(json.TestSectionId);
        //    var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();
        //    JArray getAnswers = json.FillInTheBlankAnswer;
        //    String answers = String.Empty;

        //     Find the instructor who is creating the test
        //    var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
        //    if (testSection.Test.InstructorId != instructor.Id)
        //    {
        //        return Json(new { success = false, error = "Instructor not allowed" });
        //    }
        //    else
        //    {
        //        FillInTheBlank FITBQuestion = new FillInTheBlank();
        //        FITBQuestion.Content = json.Content;
        //        FITBQuestion.Index = int.Parse((string)json.Index);
        //        FITBQuestion.PointValue = int.Parse((string)json.PointValue);
        //        FITBQuestion.TestSectionId = long.Parse((string)json.TestSectionId);
        //        FITBQuestion.QuestionType = QuestionType.FillInTheBlank;

        //        foreach (JObject x in answers)
        //        {
        //            answers += x.ToString() + " ";
        //        }

        //        _context.Questions.Add(FITBQuestion);
        //        _context.SaveChanges();
        //        return Json(new { success = true, question = FITBQuestion });
        //    }
        //}
        #endregion get different question types

        /// <summary>
        /// Update the TrueFalse question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="pointValue"></param>
        /// <param name="content"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        //[HttpPost, Route("UpdateTrueFalseQuestion")]
        //public JsonResult UpdateTrueFalseQuestion([FromBody] JObject jsonData)
        //{
        //    dynamic json = jsonData;
        //    long questionId = (long) json.questionId;
        //    int pointValue = (int) json.pointValue;
        //    string content = json.content;
        //    bool answer = (bool) json.answer;

        //    TrueFalse question = _context.TrueFalses
        //        .Include(tf => tf.TestSection)
        //            .ThenInclude(ts => ts.Test)
        //        .Where(tfq => tfq.QuestionId == questionId)
        //        .First();

        //    var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            
        //    if (question.TestSection.Test.InstructorId == instructor.Id)
        //    {
        //        question.PointValue = pointValue;
        //        question.Content = content;
        //        question.TrueFalseAnswer = answer;

        //        _context.TrueFalses.Update(question);
        //        _context.SaveChanges();
        //        return Json(new { success = true });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, error = "Instructor invalid." });
        //    }
        //}

        /// <summary>
        /// Allows the instructor to delete a test
        /// </summary>
        /// <param name="jsonData">The TestId</param>
        /// <returns></returns>
        [HttpPost, Route("DeleteTest")]
        public JsonResult DeleteTest([FromBody] JObject jsonData)
        {

            return Json(new { success = true });
        }

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
                return Json(new { success = false, error = "Instructor not allowed" });
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
                return Json(new { success = false, error = "Instructor not allowed" });
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
            switch (questionType)
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
                default:
                    return Json(new { success = false, error = "Please select a course" });
            }

            test.TestSections.Add(NewSection);
            _context.SaveChanges();


            return Json(new { success = true, sectionId = NewSection.TestSectionId, instructions = NewSection.SectionInstructions, sectionType = NewSection.QuestionType, header = sectionHeader });
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
            bool isTimeUnlimited = bool.Parse((string)json.IsTimeUnlimited);
            int timeLimit = json.TimeLimit;
            JArray studentIds = json.Students;
            JArray sectionIds = json.Sections;

            // Parsing the date
            string startDateTime = startEndDateTime.Substring(0, startEndDateTime.IndexOf(" - "));
            string endDateTime = startEndDateTime.Substring(startEndDateTime.IndexOf(" - ") + 3);
            DateTime parsedStartDateTime = DateTime.ParseExact(startDateTime, "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime parsedEndDateTime = DateTime.ParseExact(endDateTime, "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);

            // Error checking variables
            JObject errorMessages = new JObject();
            int errorCount = 0;

            if (parsedStartDateTime < DateTime.Now)
            {
                errorMessages.Add(new JProperty("DateTimeError", "The test date must be set to today's date or further."));
                errorCount++;
            }
            if (timeLimit <= 0)
            {
                errorMessages.Add(new JProperty("TimeLimitError", "The test time must be set and must be greater than 0. "));
                errorCount++;
            }
            if (studentIds.Count <= 0 && sectionIds.Count <= 0)
            {
                errorMessages.Add(new JProperty("StudentSectionErrorMessage", "The test must be assigned at least one section or at least one student"));
                errorCount++;
            }
            if (errorCount > 0)
            {
                return Json(new { success = false, ErrorMessage = errorMessages });
            }


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
                testScheduleJObject.Availability = testSchedule.StartTime.ToString("MM / dd / yyyy hh: mm tt") + " - " + testSchedule.EndTime.ToString("MM / dd / yyyy hh: mm tt");
                testScheduleJObject.TimeLimit = testSchedule.IsTimeUnlimited ? "Unlimited" : testSchedule.TimeLimit.ToString() + " minutes";

                // Checks if all students from a section were assigned
                List<int> entireSectionsAssigned = new List<int>();
                foreach (Section section in sections)
                {
                    var sectionStudentsIds = section.Enrollments.Select(e => e.StudentId).ToList();

                    if (StudentsAssignedIds.Intersect(sectionStudentsIds).Count() == sectionStudentsIds.Count())
                    {
                        entireSectionsAssigned.Add(section.SectionNumber);
                        StudentsAssignedIds = StudentsAssignedIds.Except(sectionStudentsIds).ToList();
                    }
                }

                if (entireSectionsAssigned.Any())
                {
                    testScheduleJObject.Assigmnet += "Sections " + String.Join(", ", entireSectionsAssigned.ToArray());
                }

                // Sets TestScheduleId
                testScheduleJObject.TestScheduleId = testSchedule.TestScheduleId;

                schedules.Add(testScheduleJObject);
                
            }

            if(schedules.Any())
            {
                return Json(new { success = true, schedules });
            }
            


            return Json(new { success = true, schedules = "none" });
        }

        // Gets all the students in a section
        [HttpGet, Route("GetStudentsInSection/{SectionId}")]
        public JsonResult GetSection(long SectionId)
        {

            var enrollments = _context.Enrollments.Include(e => e.ApplicationUser).Where(e => e.SectionId == SectionId).ToList();
            List<JObject> students = new List<JObject>();

            foreach (Enrollment enrollment in enrollments)
            {
                dynamic student = new JObject();
                student.FirstName = enrollment.ApplicationUser.FirstName;
                student.LastName = enrollment.ApplicationUser.LastName;
                student.Id = enrollment.ApplicationUser.Id;
                students.Add(student);
            }

            return Json(students);
        }

        // Gets all test Section of a test
        [HttpGet, Route("GetTestSections/{TestId}")]
        public JsonResult GetTestSections(long TestId)
        {
            // Gets the test
            var test = _context.Tests.Include(t => t.TestSections).Where(t => t.TestId == TestId).First();

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            // Check if the test belongs to the logged in instructor
            if (test.InstructorId == instructor.Id)
            {
                var testSections = test.TestSections;
                List<JObject> jTestSections = new List<JObject>();

                foreach (var testSection in testSections)
                {
                    dynamic jTestSection = new JObject();
                    jTestSection.sectionId = testSection.TestSectionId;
                    jTestSection.instructions = testSection.SectionInstructions;
                    jTestSection.sectionType = testSection.QuestionType;
                    jTestSection.header = DefaultTestSectionText.Header.Get(testSection.QuestionType);

                    //  Fetch the questions for each test section

                    switch (testSection.QuestionType)
                    {
                        case QuestionType.TrueFalse:
                            var TFquestions = _context.TrueFalses.Where(q => q.TestSectionId == testSection.TestSectionId).ToList();
                            JArray jTFquestios = new JArray();

                            foreach (var question in TFquestions)
                            {
                                dynamic jquestion = new JObject();
                                jquestion.questionId = question.QuestionId;
                                jquestion.pointValue = question.PointValue;
                                jquestion.content = question.Content;
                                jquestion.answer = question.TrueFalseAnswer;

                                jTFquestios.Add(jquestion);
                            }

                            jTestSection.questions = jTFquestios;
                            break;

                        case QuestionType.MultipleChoice:
                            var MCquestions = _context.Questions.Include(q => q.MultipleChoiceAnswers).Where(q => q.TestSectionId == testSection.TestSectionId).ToList();
                            JArray jMCquestios = new JArray();

                            foreach (var question in MCquestions)
                            {
                                dynamic jquestion = new JObject();

                                jquestion.pointValue = question.PointValue;
                                jquestion.content    = question.Content;
                                jquestion.questionId = question.QuestionId;
                                jquestion.sectionId  = question.TestSectionId;

                                JArray MCOptions = new JArray();
                                foreach (var option in question.MultipleChoiceAnswers)
                                {
                                    dynamic MCOption = new JObject();
                                    MCOption.multipleChoiceAnswerId = option.MultipleChoiceAnswerId;
                                    MCOption.isCorrect = option.IsCorrect;
                                    MCOption.optionContent = option.MultipleChoiceAnswerOption;

                                    MCOptions.Add(MCOption);
                                }
                                jquestion.multipleChoiceAnswers = MCOptions;

                                jMCquestios.Add(jquestion);
                            }

                            jTestSection.questions = jMCquestios;
                            break;
                    }
                    jTestSections.Add(jTestSection);
                }

                return Json(new { success = true, testSections = jTestSections });
            }
            else
            {
                return Json(new { success = false, error = "Instructor not alowed" });
            }
        }

        [HttpPost, Route("DeleteSectionSchedule")]
        public JsonResult DeleteSectionSchedule([FromBody] long sectionScheduleId)
        {
            var sectionSchedule = _context.TestSchedules.Find(sectionScheduleId);
            var test = _context.Tests.Find(sectionSchedule.TestId);

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            if (test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not allowed" });
            }
            else
            {
                _context.TestSchedules.Remove(sectionSchedule);
                _context.SaveChanges();
                return Json(new { success = true });
            }
        }

        // Updates a point poitvalue for a question
        [HttpPost, Route("UpdateQuestionPointValue")]
        public JsonResult UpdateQuestionPointValue([FromBody] JObject jsonData)
        {
            // Receiving the data
            dynamic json = jsonData;
            long questionId = (long) json.questionId;
            int pointValue = (int) json.pointValue;

            // load the question and instructor
            var question = _context.Questions.Include(q => q.TestSection).ThenInclude(ts => ts.Test).Where(q => q.QuestionId == questionId).First();
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var oldPointValue = question.PointValue;
            // Check if the question belongs to the instructor
            if (instructor.Id == question.TestSection.Test.InstructorId)
            {
                if(pointValue >= 1)
                {
                    question.PointValue = pointValue;
                    _context.SaveChanges();
                    return Json(new { success = true, oldPointValue });
                }
                else
                {
                    return Json(new { success = false, error = "The point value must be grater or equal to one" });
                }
            }
            else
            {
                return Json(new { success = false, error = "Anathorized action" });
            }
        }

        // Deletes a question from a section
        [HttpPost, Route("DeleleQuestion")]
        public JsonResult DeleleQuestion([FromBody] JObject jsonData)
        {
            // Receiving the data
            dynamic json = jsonData;
            long questionId = (long)json.questionId;

            // load the question and instructor
            var question = _context.Questions.Include(q => q.TestSection).ThenInclude(ts => ts.Test).Where(q => q.QuestionId == questionId).First();
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            if (instructor.Id == question.TestSection.Test.InstructorId)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "Anathorized action" });
            }
            
        }

        [HttpPost, Route("DeleteTestSection")]
        public JsonResult DeleteTestSection([FromBody] JObject jsonData)
        {
            // Receiving the data
            dynamic json = jsonData;
            long sectionId = (long)json.sectionId;

            var section = _context.TestSections
                .Include(ts => ts.Questions)
                .Include(ts => ts.Test)
                .Where(ts => ts.TestSectionId == sectionId)
                .First();

            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            if (instructor.Id == section.Test.InstructorId)
            {
                _context.Questions.RemoveRange(section.Questions);
                _context.TestSections.Remove(section);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            else
            {

                return Json(new { success = false, error = "Anathorized action" });
            }
           
        }
    }
}