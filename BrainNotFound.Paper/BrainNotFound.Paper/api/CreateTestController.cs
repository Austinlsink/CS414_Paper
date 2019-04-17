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
        public CreateTestController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        // TODO Lacy - Create controller for the matching question

        #region get different question types

        /// <summary>
        /// Allows the instructor to create a true false question
        /// </summary>
        /// <param name="jsonData">JObject that contains all of the question information</param>
        /// <returns>Json result of either true if the question is successfully created; otherwise, false</returns>        
        [HttpPost, Route("NewTrueFalseQuestion")]
        public JsonResult NewTrueFalseQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long testSectionId = json.testSectionId;
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();

            // Find the instructor who is creating the test to verify that it is the correct person
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

        /// <summary>
        /// Allows the instructor to update a true false question
        /// </summary>
        /// <param name="jsonData">JObject that contains all of the information for the question</param>
        /// <returns>Json result of either true if the true false question was updated; otherwise, false</returns>
        [HttpPost, Route("UpdateTrueFalseQuestion")]
        public JsonResult UpdateTrueFalseQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long questionId = (long)json.questionId;
            int pointValue = (int)json.pointValue;
            string content = json.content;
            bool answer = (bool)json.answer;

            TrueFalse question = _context.TrueFalses
                .Include(tf => tf.TestSection)
                    .ThenInclude(ts => ts.Test)
                .Where(tfq => tfq.QuestionId == questionId)
                .First();

            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            if (question.TestSection.Test.InstructorId == instructor.Id)
            {
                question.PointValue = pointValue;
                question.Content = content;
                question.TrueFalseAnswer = answer;

                _context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "Instructor invalid." });
            }
        }

        /// <summary>
        /// Allows the instructor to create a matching question
        /// </summary>
        /// <param name="jsonData">JObject that contains all of the information for the question</param>
        /// <returns>Json result of either true if the question is successfully created; otherwise, false</returns>
        [HttpPost, Route("NewMatchingQuestion")]
        public IActionResult NewMatchingQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long testSectionId = json.testSectionId;
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();
            JArray matchingGroups = json.matchingGroups;

            // Find the instructor who is creating the test and verify that it is the correct instructor
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (testSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not allowed" });
            }
            else
            {
                Question matchingQuestion = new Question
                {
                    Content = json.questionContent,
                    //Index = int.Parse((string)json.Index),
                    PointValue = json.pointValue,
                    TestSectionId = testSectionId,
                    QuestionType = QuestionType.Matching,
                    MatchingQuestionSides = new List<MatchingQuestionSide>()

                };

                // Parse all groups
                foreach (JObject matchingGroup in matchingGroups)
                {
                    dynamic matchGroup = matchingGroup;
                    JArray Jmatches = matchGroup.matches;

                    // Create the answer to the gruop
                    MatchingAnswerSide matchingAnswerSide = new MatchingAnswerSide
                    {
                        MatchingAnswer = matchGroup.matchAnswer,
                        question = matchingQuestion
                    };

                    var matchingQuestionSides = new List<MatchingQuestionSide>();

                    foreach (string matchContent in Jmatches)
                    {
                        var matchingQuestionSide = new MatchingQuestionSide()
                        {
                            Content = matchContent,
                            matchingAnswerSide = matchingAnswerSide,
                        };

                        matchingQuestion.MatchingQuestionSides.Add(matchingQuestionSide);
                    }
                }

                _context.Questions.Add(matchingQuestion);
                _context.SaveChanges();

                var question = matchingQuestion.GetJsonMatching();

                return Json(new { success = true, question });
            }
        }

        /// <summary>
        /// Allows the instructor to update the matching questions
        /// </summary>
        /// <param name="jsonData">JObject of all the information for the question</param>
        /// <returns>Json result of either true if the question is successfully updated; otherwise, false</returns>
        [HttpPost, Route("UpdateMatchingQuestion")]
        public IActionResult UpdateMatchingQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long questionId = json.questionId;
            string content = json.questionContent;
            int pointValue = json.pointValue;
            var matchingQuestion = _context.Questions
                .Include(q => q.MatchingAnswerSides)
                .Include(q => q.MatchingQuestionSides)
                .Include(q => q.TestSection)
                    .ThenInclude(ts => ts.Test)
                .Where(q => q.QuestionId == questionId)
                .First();

            JArray matchingGroups = json.matchingGroups;

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            if (matchingQuestion.TestSection.Test.InstructorId != instructor.Id)
            {
                return Json(new { success = false, error = "Instructor not allowed" });
            }
            else
            {
                _context.MatchingAnswerSides.RemoveRange(matchingQuestion.MatchingAnswerSides);
                _context.MatchingQuestionSides.RemoveRange(matchingQuestion.MatchingQuestionSides);

                // Update Common Data
                matchingQuestion.Content = content;
                matchingQuestion.PointValue = pointValue;

                // Parse all groups
                foreach (JObject matchingGroup in matchingGroups)
                {
                    dynamic matchGroup = matchingGroup;
                    JArray Jmatches = matchGroup.matches;

                    // Create the answer to the gruop
                    MatchingAnswerSide matchingAnswerSide = new MatchingAnswerSide
                    {
                        MatchingAnswer = matchGroup.matchAnswer,
                        question = matchingQuestion
                    };

                    var matchingQuestionSides = new List<MatchingQuestionSide>();

                    foreach (string matchContent in Jmatches)
                    {
                        var matchingQuestionSide = new MatchingQuestionSide()
                        {
                            Content = matchContent,
                            matchingAnswerSide = matchingAnswerSide,
                        };

                        matchingQuestion.MatchingQuestionSides.Add(matchingQuestionSide);
                    }
                }

                _context.SaveChanges();

                var question = matchingQuestion.GetJsonMatching();

                return Json(new { success = true, question });
            }
        }

        /// <summary>
        /// Allows the instructor to create a new multiple choice question
        /// </summary>
        /// <param name="jsonData">JObject that contains all of the question information</param>
        /// <returns>Json result of either true if the question is successfully added; otherwise, false</returns>
        [HttpPost, Route("NewMultipleChoiceQuestion")]
        public JsonResult NewMultipleChoiceQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long testSectionId = json.testSectionId;
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();
            JArray MCAnswers = json.multipleChoiceAnswers;

            // Find the instructor who is creating the test and verify that it is the correct instructor
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

                var question = MCQuestion.GetJsonMultipleChoice();

                return Json(new { success = true, question });
            }
        }

        /// <summary>
        /// Allows the instructor to update a multiple choice question
        /// </summary>
        /// <param name="jsonData">JObject of all the information for the question</param>]
        /// <returns>Json result of either true if the question was successfully updated; otherwise, false</returns>
        [HttpPost, Route("UpdateMultipleChoiceQuestion")]
        public JsonResult UpdateMultipleChoiceQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long questionId = json.questionId;
            string content = json.questionContent;
            int pointValue = json.pointValue;

            JArray MCAnswers = json.multipleChoiceAnswers;

            var multipleChoiceQuestion = _context.Questions.Include(q => q.MultipleChoiceAnswers).Where(q => q.QuestionId == questionId).First();

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            // Resets the question options
            _context.MultipleChoiceAnswers.RemoveRange(multipleChoiceQuestion.MultipleChoiceAnswers);

            // Creates the annswer options
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

            multipleChoiceQuestion.MultipleChoiceAnswers = MCAList;
            multipleChoiceQuestion.PointValue = pointValue;
            multipleChoiceQuestion.Content = content;

            _context.SaveChanges();

            var question = multipleChoiceQuestion.GetJsonMultipleChoice();

            return Json(new { success = true, question });
        }

        /// <summary>
        /// Allows the instructor to create a new essay question
        /// </summary>
        /// <param name="jsonData">JObject that contains all of the essay question information</param>
        /// <returns>Json result of true once the essay question is added to the DB</returns>
        [HttpPost, Route("NewEssayQuestion")]
        public JsonResult NewEssayQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long testSectionId = json.testSectionId;
            var testSection = _context.TestSections.Include(s => s.Test).Where(x => x.TestSectionId == testSectionId).First();

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            // Creates the new essay question with the view's data
            Essay newEssayQuestion = new Essay
            {
                Content = json.questionContent,
                //Index = json.Index,
                PointValue = json.pointValue,
                TestSectionId = testSectionId,
                QuestionType = QuestionType.Essay,
                ExpectedEssayAnswer = json.expectedAnswer
            };

            // saves question to db
            _context.Essays.Add(newEssayQuestion);
            _context.SaveChanges();

            // Returns question to the view0
            var question = newEssayQuestion.ToJObject();

            return Json(new { success = true, question });
        }

        /// <summary>
        /// Allows the instructor to update an essay question
        /// </summary>
        /// <param name="jsonData">JOject that contains all of the information for the question</param>
        /// <returns>Json object of true once the essay question is updated</returns>
        [HttpPost, Route("UpdateEssayQuestion")]
        public JsonResult UpdateEssayQuestion([FromBody] JObject jsonData)
        {
            dynamic json = jsonData;
            long questionId = json.questionId;
            var essayQuestion = _context.Essays.Find(questionId);

            // Find the instructor who is creating the test
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            essayQuestion.PointValue = json.pointValue;
            essayQuestion.Content = json.content;
            essayQuestion.ExpectedEssayAnswer = json.expectedAnswer;

            // saves question to db
            _context.SaveChanges();

            return Json(new { success = true });

        }

        /// <summary>
        /// Allows the instructor to create  a new fill in the blank question
        /// </summary>
        /// <param name="jsonData">JObject that contains all of the fill in the blank question information</param>
        /// <returns>Json result of either true if the question was added to the DB; otherwise, false</returns>
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
        /// Allows the instructor to delete a test section 
        /// </summary>
        /// <param name="jsonData">JObject that contains the TestSectionId to be deleted</param>
        /// <returns>Json resul of either true if the test section was successfully deleted; otherwise, false</returns>
        [HttpPost, Route("DeleteSectionTestId")]
        public JsonResult DeleteSectionTestId([FromBody] JObject jsonData)
        {
            // parse the information from the JObject
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
        /// Allows the instructor to update the section instructions
        /// </summary>
        /// <param name="jsonData">JObject of the new infromation</param>
        /// <returns>Json result of either true if the instructions were updated; otherwise, false</returns>
        [HttpPost, Route("UpdateSectionInstruction")]
        public JsonResult UpdateSectionInstruction([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
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

        /// <summary>
        /// Gets all of the questions for a specific test section
        /// </summary>
        /// <param name="testSectionId">Search criteria for the specified test section</param>
        /// <returns>Json object of all the questions</returns>
        [HttpGet, Route("GetQuestionsInSection/{testSectionId}")]
        public JsonResult GetQuestionsInSection(long testSectionId)
        {
            var questions = _context.Questions.Where(q => q.TestSectionId == testSectionId).ToList();

            return Json(questions);
        }

        /// <summary>
        /// Allows the instructor to create a test section
        /// </summary>
        /// <param name="jsonData">JObject of all of the information for the test section</param>
        /// <returns>Jresult of either true if the test section was added; otherwise, false</returns>
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
        /// Allows the instructor to create a new test schedule
        /// </summary>
        /// <param name="jsonData">JObject of all the information for the test schedule</param>
        /// <returns>Json result of either true if the test schedule was successfully created; otherwise, false</returns>
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
            
            // Assigns individual students
            for(int i = 0; i < studentIds.Count; i++)
            {
                string id = studentIds[i].ToString();

                if (!newTestSchedule.StudentTestAssignments.Where(x => x.StudentId == id).Any())
                {
                    newTestSchedule.StudentTestAssignments.Add(new StudentTestAssignment()
                    {
                        StudentId = id
                    });
                }
            }

            _context.TestSchedules.Add(newTestSchedule);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        /// <summary>
        /// Gets the test schedules for the specific instructor
        /// </summary>
        /// <param name="testId">Search criteria for the specific test</param>
        /// <returns>Json result of either true and the test schedules, or false</returns>
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
                testScheduleJObject.Availability = testSchedule.GetAvailability();
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

            if (schedules.Any())
            {
                return Json(new { success = true, schedules });
            }

            return Json(new { success = true, schedules = "none" });
        }

        /// <summary>
        /// Gets all of the students associated with a section
        /// </summary>
        /// <param name="SectionId">Search criteria for a specific section</param>
        /// <returns>Json object of all the students</returns>
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

        /// <summary>
        /// Gets all of the test sections for a specific test
        /// </summary>
        /// <param name="TestId">Search criteria for the specific test</param>
        /// <returns>Json result of either true and all of the tests sections, or false</returns>
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
                                jTFquestios.Add(question.ToJObject());
                            }

                            jTestSection.questions = jTFquestios;
                            break;

                        case QuestionType.MultipleChoice:
                            var MCquestions = _context.Questions.Include(q => q.MultipleChoiceAnswers).Where(q => q.TestSectionId == testSection.TestSectionId).ToList();
                            JArray jMCquestios = new JArray();

                            foreach (var question in MCquestions)
                            {
                                jMCquestios.Add(question.GetJsonMultipleChoice());
                            }

                            jTestSection.questions = jMCquestios;
                            break;
                        case QuestionType.Essay:
                            var essayQuestions = _context.Essays.Where(q => q.TestSectionId == testSection.TestSectionId).ToList();
                            JArray jEssayQuestios = new JArray();

                            foreach (var question in essayQuestions)
                            {
                                jEssayQuestios.Add(question.ToJObject());
                            }

                            jTestSection.questions = jEssayQuestios;
                            break;
                        case QuestionType.Matching:
                            var matchingQuestions = _context.Questions
                                .Include(q => q.MatchingAnswerSides)
                                .Include(q => q.MatchingQuestionSides)
                                .Where(q => q.TestSectionId == testSection.TestSectionId)
                                .ToList();

                            JArray jMatchingQuestios = new JArray();
                            foreach (var question in matchingQuestions)
                            {
                                jMatchingQuestios.Add(question.GetJsonMatching());
                            }
                            jTestSection.questions = jMatchingQuestios;
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

        /// <summary>
        /// Allows the instructor to delete a section schedule
        /// </summary>
        /// <param name="sectionScheduleId">Seaerch criteria for a specific section schedule</param>
        /// <returns>Json result of either true if successfully removed; otherwise, false</returns>
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

        /// <summary>
        /// Allows the instructor to update the question point value
        /// </summary>
        /// <param name="jsonData">JObject of all the information needed to update the point value</param>
        /// <returns>Json result of either true if successfully updated; otherwise, false</returns>
        [HttpPost, Route("UpdateQuestionPointValue")]
        public JsonResult UpdateQuestionPointValue([FromBody] JObject jsonData)
        {
            // Receiving the data
            dynamic json = jsonData;
            long questionId = (long)json.questionId;
            int pointValue = (int)json.pointValue;

            // load the question and instructor
            var question = _context.Questions.Include(q => q.TestSection).ThenInclude(ts => ts.Test).Where(q => q.QuestionId == questionId).First();
            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var oldPointValue = question.PointValue;
            // Check if the question belongs to the instructor
            if (instructor.Id == question.TestSection.Test.InstructorId)
            {
                if (pointValue >= 1)
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

        /// <summary>
        /// Allows the instructor to delete a question from a section
        /// </summary>
        /// <param name="jsonData">JObject of all the information needed to delete a question from a section</param>
        /// <returns>Json result of either true if the question is successfully deleted; otherwise, false</returns>
        [HttpPost, Route("DeleleQuestion")]
        public JsonResult DeleleQuestion([FromBody] JObject jsonData)
        {
            // Parse the information from the JObject
            dynamic json = jsonData;
            long questionId = (long)json.questionId;

            // load the question and instructor
            var question = _context.Questions
                .Include(q => q.TestSection)
                    .ThenInclude(ts => ts.Test)
                .Include(q => q.MatchingAnswerSides)
                .Include(q => q.MatchingQuestionSides)
                .Where(q => q.QuestionId == questionId)
                .First();

            var instructor = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            if (instructor.Id == question.TestSection.Test.InstructorId)
            {
                if(question.QuestionType == QuestionType.Matching)
                {
                    _context.MatchingQuestionSides.RemoveRange(question.MatchingQuestionSides);
                    _context.MatchingAnswerSides.RemoveRange(question.MatchingAnswerSides);
                }

                _context.Questions.Remove(question);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, error = "Unauthorized action" });
            }
        }

        /// <summary>
        /// Allows the instructor to delete a ttest section
        /// </summary>
        /// <param name="jsonData">JObject of all the information needed to delete a test section</param>
        /// <returns>Json result of either true if successfully deleted; otherwise, false</returns>
        [HttpPost, Route("DeleteTestSection")]
        public JsonResult DeleteTestSection([FromBody] JObject jsonData)
        {
            // parse the information from the JObject
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
                if(section.QuestionType == QuestionType.Matching)
                {
                    foreach(var question in section.Questions)
                    {
                        var currentQuestion = _context.Questions.Include(q => q.MatchingAnswerSides).Include(q => q.MatchingQuestionSides).Where(q => q.QuestionId == question.QuestionId).First();
                        _context.MatchingQuestionSides.RemoveRange(currentQuestion.MatchingQuestionSides);
                        _context.MatchingAnswerSides.RemoveRange(currentQuestion.MatchingAnswerSides);
                    }
                }
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