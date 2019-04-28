using System;
using System.Linq;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainNotFound.Paper.api
{
    [Route("api/Tests")]
    [ApiController]
    public class TestController : Controller
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
        public TestController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        /// <summary>
        /// Allows the instructor to delete a test if the start time is in the future
        /// </summary>
        /// <param name="testId">Search criteria for a specific testId</param>
        /// <returns>Json result of either true if the test is deleted; otherwise, false </returns>
        [HttpPost, Route("DeleteTest")]
        public JsonResult DeleteTest([FromBody] long testId)
        {
            string ProgressMessage = String.Empty;
            string PastMessage = String.Empty;
            string SuccessMessage = String.Empty;
            int countError = 0;


            var MatchingQuestionSides = _context.MatchingQuestionSides
                .Include(mqs => mqs.Question)
                    .ThenInclude(q => q.TestSection)
                .Where(mqs => mqs.Question.TestSection.TestId == testId);

            _context.MatchingQuestionSides.RemoveRange(MatchingQuestionSides);

            var questions = _context.Questions.Include(q => q.TestSection).Where(q => q.TestSection.TestId == testId);
            _context.Questions.RemoveRange(questions);

            var testSchedules = _context.TestSchedules.Where(ts => ts.TestId == testId);
            _context.TestSchedules.RemoveRange(testSchedules);

            var test = _context.Tests.Find(testId);
            _context.Tests.Remove(test);


            //var test = _context.Tests.Include(x => x.TestSchedules).Where(x => x.TestId == testId).First();

            //var testSectionSchedules = _context.TestSchedules.Where(ts => ts.TestId == test.TestId).ToList();

            //var questions = _context.Questions.Include(x => x.TestSection).Where(x => testSectionSchedules.Any(y => y.TestId == x.TestSection.TestId)).ToList();


            //var testSections = _context.TestSections.Where(ts => ts.TestId == testId);

            //for (int i = 0; i < questions.Count; i++)
            //{
            //    _context.Questions.Remove(questions[i]);
            //}

            //for (int i = 0; i < testSectionSchedules.Count; i++)
            //{
            //    _context.TestSchedules.Remove(testSectionSchedules[i]);
            //}

            //_context.TestSections.RemoveRange(testSections);
            
            _context.SaveChanges();

            return Json(new { success = true, message = "The test was successfully deleted" });

        }
    }
}