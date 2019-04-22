using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BrainNotFound.Paper.api
{
    [Authorize(Roles = "Instructor")]
    [Route("api/GradeTest")]
    public class GradeTestController : Controller
    {
        #region Initialize controllers

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        public GradeTestController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("GradeQuestion")]
        public JsonResult GetSection([FromBody]JObject jsonData)
        {
            dynamic json = jsonData;
            long answerId = json.answerId;
            string comment = json.comment;
            int pointsEarned = json.pointsEarned;

            if(pointsEarned < 0)
            {
                return Json(new { success = false, errorMessage = "Invalid point value" });
            }
            var studentEssayAnswer = _context.StudentEssayAnswers.Find(answerId);
            studentEssayAnswer.Comments = comment;
            studentEssayAnswer.PointsEarned = pointsEarned;

            _context.SaveChanges();

            return Json(new {success = true });
        }


    }
}