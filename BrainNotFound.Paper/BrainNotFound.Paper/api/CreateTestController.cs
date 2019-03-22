using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
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


        [Route("GetStudentsInSection/{SectionId}")]
        public JsonResult GetSection(long SectionId)
        {

            var enrollments = _context.Enrollments.Include(e => e.ApplicationUser).Where(e => e.SectionId == SectionId).ToList();
            List<JObject> students = new List<JObject>();

            foreach(Enrollment enrollment in enrollments)
            {
                dynamic student = new JObject();
                student.FirstName = enrollment.ApplicationUser.FirstName;
                student.LastName = enrollment.ApplicationUser.LastName;
                student.Id = enrollment.ApplicationUser.Id;
                students.Add(student);
            }

            return Json(students);
        }       
    }
}