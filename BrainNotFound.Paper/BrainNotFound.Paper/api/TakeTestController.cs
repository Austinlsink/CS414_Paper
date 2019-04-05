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

        [HttpPost, Route("SaveTrueFalseAnswer")]
        public JsonResult SaveTrueFalseAnswer(JObject data)
        {
            dynamic trueFalseInfo = data;
            long questionId = trueFalseInfo.QuestionId;
            string answer = trueFalseInfo.Answer;
            string studentId = trueFalseInfo.StudentId;
            long testScheduleId = trueFalseInfo.TestScheduleId;



            return Json(new { success = true });
        }

    }
}