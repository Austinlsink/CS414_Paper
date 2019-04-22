using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BrainNotFound.Paper.api
{
    [Authorize(Roles = "Student")]
    [Route("api/Tests")]
    [ApiController]
    public class TakeTestController : Controller
    {
        #region Initialize controllers

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        public TakeTestController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        /// <summary>
        /// Allows a student to submit his test once he's finished
        /// </summary>
        /// <param name="JsonData">JObject: TestScheduleId, Pledge</param>
        /// <returns>Json result of either true if the test was successfully submitted; otherwise, false</returns>
        [HttpPost, Route("SubmitTest")]
        public async Task<JsonResult> SubmitTest(JObject JsonData)
        {
            // Parse the information from the JObject
            dynamic data = JsonData;
            long testScheduleId = data.TestScheduleId;
            string pledge = data.Pledge;
            bool isPledgeSigned = pledge == String.Empty ? false : true;

            ApplicationUser student = await _userManager.FindByNameAsync(User.Identity.Name);

            var studentTestAssignment = _context.StudentTestAssignments.Where(x => x.StudentId == student.Id && x.TestScheduleId == testScheduleId).FirstOrDefault();
            if (studentTestAssignment == null)
            {
                return Json(new { success = false });
            }
            else
            {
                studentTestAssignment.Submitted = true;
                studentTestAssignment.Signed = isPledgeSigned;
                _context.SaveChanges(); 
                return Json(new { success = true });
            }
        }

        /// <summary>
        /// Confirms if all the questions are answered when the test is submitted.
        /// </summary>
        /// <param name="jsonObject">JObject: TestScheduleId</param>
        /// <returns>Json result of either true if all the questions were answered; otherwise, false</returns>
        [HttpPost, Route("ConfirmAllQuestionsAnswered")]
        public JsonResult ConfirmAllQuestionsAnswered(JObject JsonData)
        {
            // Parse the information from the JObject
            dynamic data = JsonData;
            long testScheduleId = data.TestScheduleId;

            var testSchedule = _context.TestSchedules.Include(x => x.Test).ThenInclude(x => x.TestSections).Where(x => x.TestScheduleId == testScheduleId).First();
            var testSections = testSchedule.Test.TestSections.ToList();

            var allQuestions = _context.Questions.ToList();
            List<Question> testSectionQuestions = new List<Question>();

            // Grab all of the questions for each test section
            foreach(TestSection ts in testSections)
            {
                foreach(Question q in allQuestions)
                {
                    if (q.TestSectionId == ts.TestSectionId)
                    {
                        testSectionQuestions.Add(q);
                    }
                }
            }

            // Grab all of the student answers
            var allStudentAnswers = _context.StudentAnswers.Where(x => x.TestScheduleId == testSchedule.TestScheduleId).ToList();
            
            if(testSectionQuestions.Count == allStudentAnswers.Count)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        #region save student answers

        /// <summary>
        /// Saves or updates the matching choice answer that the student selects
        /// </summary>
        /// <param name="JsonData">JObject: questionId, answer, TestScheduleId, MultipleChoiceAnswerId, IsSelected</param>
        /// <returns>Json result of true once the question is either saved or updated</returns>
        [HttpPost, Route("SaveMatchingChoiceAnswer")]
        public JsonResult SaveMatchingChoiceAnswer(JObject JsonData)
        {
            dynamic matchingInfo = JsonData;
            long testScheduleId = (long)matchingInfo.TestScheduleId;
            long matchingAnswerSideId = (long)matchingInfo.MatchingAnswerSideId;
            long matchingQuestionSideId = (long)matchingInfo.MatchingQuestionSideId;
            long questionId = (long)matchingInfo.QuestionId;

            // Grab the student information
            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            // Find the student answer
            var studentAnswer = _context.StudentMatchingAnswers.Where(x => x.MatchingAnswerSideId == matchingAnswerSideId && x.MatchingQuestionSideId == matchingQuestionSideId).FirstOrDefault();

            if (studentAnswer == null)
            {
                StudentAnswer newStudentAnswer = new StudentAnswer()
                {
                    QuestionId = questionId,
                    TestScheduleId = testScheduleId,
                    StudentId = student.Id
                };
                _context.StudentAnswers.Add(newStudentAnswer);

                StudentMatchingAnswer newStudentMatchingAnswer = new StudentMatchingAnswer()
                {
                    AnswerId = newStudentAnswer.AnswerId,
                    MatchingAnswerSideId = matchingAnswerSideId,
                    MatchingQuestionSideId = matchingQuestionSideId
                };

                _context.StudentMatchingAnswers.Add(newStudentMatchingAnswer);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            else
            {
                StudentAnswer newStudentAnswer = new StudentAnswer()
                {
                    QuestionId = questionId,
                    TestScheduleId = testScheduleId,
                    StudentId = student.Id
                };

                studentAnswer.AnswerId = newStudentAnswer.AnswerId;
                _context.StudentMatchingAnswers.Update(studentAnswer);
                return Json(new { success = true });
            }
        }

        /// <summary>
        /// Saves or updates the multiple choice answer that the student selects
        /// </summary>
        /// <param name="JsonData">JObject: questionId, answer, TestScheduleId, MultipleChoiceAnswerId, IsSelected</param>
        /// <returns>Json result of true once the question is either saved or updated</returns>
        [HttpPost, Route("SaveMultipleChoiceAnswer")]
        public JsonResult SaveMultipleChoiceAnswer(JObject JsonData)
        {
            // Parse the information from the JObject
            dynamic multipleChoiceInfo = JsonData;
            long questionId = (long) multipleChoiceInfo.QuestionId;
            string answer = multipleChoiceInfo.Answer;
            long testScheduleId = (long) multipleChoiceInfo.TestScheduleId;
            long mcAnswerId = (long) multipleChoiceInfo.MCAnswerId;
            bool isSelected = (bool) multipleChoiceInfo.IsSelected;

            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var studentAnswer = _context.StudentAnswers.Include(x => x.StudentMultipleChoiceAnswers).Where(x => x.QuestionId == questionId && x.TestScheduleId == testScheduleId && x.StudentId == student.Id).FirstOrDefault();
            
            // If studentAnswer is empty, create a new student answer and add a list of StudentMultipleChoiceAnswer
            if (studentAnswer == null)
            {
                StudentAnswer newStudentAnswer = new StudentAnswer()
                {
                    QuestionId = questionId,
                    TestScheduleId = testScheduleId,
                    StudentId = student.Id
                };

                List<StudentMultipleChoiceAnswer> answersGiven = new List<StudentMultipleChoiceAnswer>();
                answersGiven.Add(new StudentMultipleChoiceAnswer()
                {
                    MultipleChoiceAnswerId = mcAnswerId,
                    AnswerId = newStudentAnswer.AnswerId                    
                });

                newStudentAnswer.StudentMultipleChoiceAnswers = answersGiven;

                _context.StudentAnswers.Add(newStudentAnswer);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            // If the studentAnswer already exists, modify the MultipleChoiceAnswer list
            else
            {
                if (isSelected)
                {
                    StudentMultipleChoiceAnswer newChoice = new StudentMultipleChoiceAnswer()
                    {
                        MultipleChoiceAnswerId = mcAnswerId,
                        AnswerId = studentAnswer.AnswerId
                    
                    };
                    studentAnswer.StudentMultipleChoiceAnswers.Add(newChoice);
                    _context.SaveChanges();
                }
                else
                {
                    var answerRetrived = _context.StudentMultipleChoiceAnswers.Where(x => x.MultipleChoiceAnswerId == mcAnswerId && x.AnswerId == studentAnswer.AnswerId).FirstOrDefault();
                    studentAnswer.StudentMultipleChoiceAnswers.Remove(answerRetrived);

                    if(studentAnswer.StudentMultipleChoiceAnswers.Count == 0)
                    {
                        _context.StudentAnswers.Remove(studentAnswer);
                    }

                    _context.SaveChanges();
                }

                return Json(new { success = true });
            }
        }

        /// <summary>
        /// Saves the student's true false answer
        /// </summary>
        /// <param name="data">JObject: QuestionId, answer, TestScheduleId</param>
        /// <returns></returns>
        [HttpPost, Route("SaveTrueFalseAnswer")]
        public JsonResult SaveTrueFalseAnswer(JObject data)
        {
            // Parse the information from the JObject
            dynamic trueFalseInfo = data;
            long questionId = (long) trueFalseInfo.QuestionId;
            bool answer = (bool) trueFalseInfo.Answer;
            long testScheduleId = (long) trueFalseInfo.TestScheduleId;

            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            var studentAnswer = _context.StudentTrueFalseAnswers.Where(x => x.QuestionId == questionId && x.TestScheduleId == testScheduleId && x.StudentId == student.Id).FirstOrDefault();
            
            if(studentAnswer == null)
            {
                StudentTrueFalseAnswer TFAnswer = new StudentTrueFalseAnswer()
                {
                    TrueFalseAnswerGiven = answer,
                    StudentId = student.Id,
                    TestScheduleId = testScheduleId,
                    QuestionId = questionId
                };

                _context.StudentAnswers.Add(TFAnswer);
                _context.SaveChanges();
            }
            else
            {
                studentAnswer.TrueFalseAnswerGiven = answer;
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// Saves and updates the student's essay answer
        /// </summary>
        /// <param name="JsonData">JObject: QuestionId, TestScheduleId, StudentEssayAnswer</param>
        /// <returns>Json result of true once the essay question is saved</returns>
        [HttpPost, Route("SaveEssayAnswer")]
        public JsonResult SaveEssayAnswer(JObject JsonData)
        {
            // Parse the information from the JObject
            dynamic multipleChoiceInfo = JsonData;
            long questionId = (long)multipleChoiceInfo.QuestionId;
            long testScheduleId = (long)multipleChoiceInfo.TestScheduleId;
            string studentEssayAnswer = multipleChoiceInfo.StudentEssayAnswer;

            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            var studentAnswer = _context.StudentEssayAnswers.Where(x => x.QuestionId == questionId && x.StudentId == student.Id && x.TestScheduleId == testScheduleId).FirstOrDefault();

            if (studentAnswer == null)
            {
                StudentEssayAnswer essayAnswer = new StudentEssayAnswer()
                {
                    TestScheduleId = testScheduleId,
                    EssayAnswerGiven = studentEssayAnswer,
                    QuestionId = questionId,
                    StudentId = student.Id
                };

                _context.StudentEssayAnswers.Add(essayAnswer);
                _context.StudentTestAssignments.Where(x => x.TestScheduleId == testScheduleId && x.StudentId == student.Id).First().ManualGradingRequired = true;
                _context.SaveChanges();
            }
            else
            {
                studentAnswer.EssayAnswerGiven = studentEssayAnswer;
                _context.SaveChanges();
            }

            return Json(new { success = true });
        }

        #endregion save student answers
    }
}