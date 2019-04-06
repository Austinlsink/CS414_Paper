using System;
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

        [HttpPost, Route("ConfirmAllQuestionsAnswered")]
        public JsonResult ConfirmAllQuestionsAnswered(JObject jsonObject)
        {
            dynamic data = jsonObject;
            long testScheduleId = (long) data.TestScheduleId;

            var allQuestions = _context.Questions.Include(x => x.TestSection);

            return Json(new { success = true });
        }

        [HttpPost, Route("SaveTrueFalseAnswer")]
        public JsonResult SaveTrueFalseAnswer(JObject data)
        {
            dynamic trueFalseInfo = data;
            long questionId = (long) trueFalseInfo.QuestionId;
            bool answer = (bool) trueFalseInfo.Answer;
            long testScheduleId = (long) trueFalseInfo.TestScheduleId;

            var student = _context.ApplicationUsers.Where(u => u.UserName == User.Identity.Name).First();

            var studentAnswer = _context.StudentTrueFalseAnswers.Where(x => x.QuestionId == questionId && x.TestScheduleId == testScheduleId && x.StudentId == student.Id).FirstOrDefault();
            

            // 
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