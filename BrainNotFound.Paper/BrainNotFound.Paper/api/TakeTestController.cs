﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        /// 
        public TakeTestController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("SubmitTest")]
        public async Task<JsonResult> SubmitTest(JObject JsonData)
        {
            dynamic data = JsonData;
            long testScheduleId = data.TestScheduleId;
           // return Json(new { success = true, message = "In the controller: " + testScheduleId });

            ApplicationUser student = await _userManager.FindByNameAsync(User.Identity.Name);

            var studentTestAssignment = _context.StudentTestAssignments.Where(x => x.StudentId == student.Id && x.TestScheduleId == testScheduleId).FirstOrDefault();
            if (studentTestAssignment == null)
            {
                return Json(new { success = false });
            }
            else
            {
                studentTestAssignment.Submitted = true;
                //_context.SaveChanges();
                return Json(new { success = true });
            }
        }

        /// <summary>
        /// Confirms if all the questions are answered. If not, it returns an error message.
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        [HttpPost, Route("ConfirmAllQuestionsAnswered")]
        public JsonResult ConfirmAllQuestionsAnswered(long testScheduleId)
        {
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


        /// <summary>
        /// Biiiiiiiimmmmmmmmaaaaaaa... it doesn't make sense
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveMultipleChoiceAnswer")]
        public JsonResult SaveMultipleChoiceAnswer(JObject data)
        {
            dynamic multipleChoiceInfo = data;
            long questionId = (long)multipleChoiceInfo.QuestionId;
            string answer = multipleChoiceInfo.Answer;
            long testScheduleId = (long)multipleChoiceInfo.TestScheduleId;

            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();
            var studentAnswer = _context.StudentAnswers.Where(x => x.QuestionId == questionId && x.TestScheduleId == testScheduleId).FirstOrDefault();
            
            if (studentAnswer == null)
            {
                
                
                
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// Saves the student's true false answer
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveTrueFalseAnswer")]
        public JsonResult SaveTrueFalseAnswer(JObject data)
        {
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
    }
}