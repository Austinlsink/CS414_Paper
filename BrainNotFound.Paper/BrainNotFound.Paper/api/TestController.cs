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
using Newtonsoft.Json.Linq;

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
        /// Delete the test as long as it is in the future
        /// </summary>
        /// <param name="jsonData">TestId</param>
        /// <returns></returns>
        [HttpPost, Route("DeleteTest")]
        public JsonResult DeleteTest([FromBody] long testId)
        {
            var test = _context.Tests.Find(testId);
            int countError = 0;
            var testSectionSchedules = _context.TestSchedules.Where(ts => ts.TestId == test.TestId).ToList();
            string ProgressErrorMessage = String.Empty;
            string SuccessMessage = String.Empty;
            string PastErrorMessage = String.Empty;

           

            foreach (TestSchedule schedule in testSectionSchedules)
            {
                // Parsing the date
                string startEndDateTime = schedule.StartTime.ToString();
                string startDateTime = startEndDateTime.Substring(0, startEndDateTime.IndexOf(" - "));
                string endDateTime = startEndDateTime.Substring(startEndDateTime.IndexOf(" - ") + 3);
                DateTime parsedStartDateTime = DateTime.ParseExact(startDateTime, "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);
                DateTime parsedEndDateTime = DateTime.ParseExact(endDateTime, "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);

                if (parsedStartDateTime < DateTime.Now)
                {
                    PastErrorMessage = "For record purposes, previously taken tests cannot be deleted.";
                    countError++;
                }
                if (parsedEndDateTime > DateTime.Now)
                {
                    SuccessMessage = "The test was successfully deleted.";
                }
                if (parsedStartDateTime.Day == DateTime.Now.Day && parsedStartDateTime.Hour == DateTime.Now.Hour)
                {
                    ProgressErrorMessage = "The test is currently in progress and cannot be deleted.";
                    countError++;
                }
            }

            if (countError == 0)
            {
                _context.Tests.Remove(test);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            else
            { return Json(new { success = true });
        }
    }
}