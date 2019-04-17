using System;
using System.Linq;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            var test = _context.Tests.Find(testId);
            int countError = 0;
            var testSectionSchedules = _context.TestSchedules.Where(ts => ts.TestId == test.TestId).ToList();
            string ProgressMessage = String.Empty;
            string PastMessage = String.Empty;
            string SuccessMessage = String.Empty;

            foreach (TestSchedule schedule in testSectionSchedules)
            {
                // Parsing the date
                if (schedule.StartTime < DateTime.Now)
                {
                    PastMessage = " For record purposes, previously taken tests cannot be deleted.";
                    countError++;
                }
            }
            if (countError == 0)
            {
                _context.Tests.Remove(test);
                _context.SaveChanges();
                return Json(new { success = true, message = "The test was successfully deleted"});
            }
            else
            {
                return Json(new { success = false, error = ProgressMessage + PastMessage});
            }
        }
    }
}