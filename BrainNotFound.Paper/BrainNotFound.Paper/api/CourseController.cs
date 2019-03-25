using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrainNotFound.Paper.api
{
    [Authorize(Roles = "Admin")]
    [Route("api/Course")]
    [ApiController]
    public class CourseController : Controller
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
        public CourseController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("New")]
        public IActionResult New([FromBody] Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();

            return Json(new { sucess = true });
        }

        [HttpPost, Route("Delete")]
        public IActionResult Delete([FromBody]long courseId)
        {
            var course = _context.Courses.Find(courseId);
            var SuceessMessage = string.Empty;

            if (_context.Sections.Where(ac => ac.CourseId == course.CourseId).Any())
            {
                var ErrorMessage = course.CourseCode + "-" + course.Name + " could not be deleted. Please delete all associated sections.";
                return Json(new { sucess = false, message = ErrorMessage });
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();
            SuceessMessage = course.CourseCode + " " + course.Name + " was successfully deleted!";

            return Json(new { success = true, message = SuceessMessage });
        }

        [HttpPost, Route("Edit/{courseId}")]
        public IActionResult Edit([FromBody]long courseId)
        {
            var course = _context.Courses.Find(courseId);

            return Json(new { success = true, code = course.CourseCode, name = course.Name, description = course.Description, creditHours = course.CreditHours, department = course.DepartmentId });
        }

        [HttpPost, Route("Save")]
        public IActionResult Save([FromBody] Course c)
        {
            var course = _context.Courses.Find(c.CourseId);
            course.CourseCode = c.CourseCode;
            course.Name = c.Name;
            course.Description = c.Description;
            course.CreditHours = c.CreditHours;

            _context.Courses.Update(course);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}