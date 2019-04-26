using System.Linq;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
        public CourseController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        /// <summary>
        /// Allows the admin to create a new course
        /// </summary>
        /// <param name="course">Course object that contains the information</param>
        /// <returns>Json object of true when the course has been added to the DB</returns>
        [HttpPost, Route("New")]
        public IActionResult New([FromBody] Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        /// <summary>
        /// Allows the admin to remove a section if there are no students attached to it
        /// </summary>
        /// <param name="courseId">Search criteria for finding the specific course</param>
        /// <returns>Json object either true if the course was successfully removed; otherwise, false.</returns>
        [HttpPost, Route("DeleteSection")]
        public JsonResult DeleteSection([FromBody]JObject JsonData)
        {
            dynamic data = JsonData;
            string departmentCode = data.DepartmentCode;
            string courseCode = data.CourseCode;
            int sectionNumber = (int)data.SectionNumber;
            long sectionId = (long)data.SectionId;

            var section = _context.Sections.Where(x => x.SectionId == sectionId).First();

            var enrollments = _context.Enrollments.Where(x => x.SectionId == section.SectionId).ToList();

            if(enrollments.Count > 0)
            {
                return Json(new { success = false, message = "Please unassign all students before removing this section"});
            }
            else
            {
                _context.Sections.Remove(section);
                _context.SaveChanges();
                return Json(new { success = true, message = $"{departmentCode} {courseCode} Section - {sectionNumber} was successfully deleted!"});
            }
        }

        /// <summary>
        /// Allows the admin to remove a course if there are no sections attached to it
        /// </summary>
        /// <param name="courseId">Search criteria for finding the specific course</param>
        /// <returns>Json object either true if the course was successfully removed; otherwise, false.</returns>
        [HttpPost, Route("Delete")]
        public IActionResult Delete([FromBody]long courseId)
        {
            var course = _context.Courses.Find(courseId);
            var SuceessMessage = string.Empty;

            if (_context.Sections.Where(ac => ac.CourseId == course.CourseId).Any())
            {
                var ErrorMessage = course.CourseCode + "-" + course.Name + " could not be deleted. Please delete all associated sections.";
                return Json(new { success = false, message = ErrorMessage });
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();
            SuceessMessage = course.CourseCode + " " + course.Name + " was successfully deleted!";

            return Json(new { success = true, message = SuceessMessage });
        }

        /// <summary>
        /// Allows the admin to edit a course's information
        /// </summary>
        /// <param name="courseId">Search criteria for finding the specific course</param>
        /// <returns>Json object of the course's information so that it can be edited</returns>
        [HttpPost, Route("Edit/{courseId}")]
        public IActionResult Edit([FromBody]long courseId)
        {
            var course = _context.Courses.Find(courseId);
            return Json(new { success = true, id = course.CourseId, code = course.CourseCode, name = course.Name, description = course.Description, creditHours = course.CreditHours, department = course.DepartmentId });
        }

        /// <summary>
        /// Allows the admin to save an edited course's information
        /// </summary>
        /// <param name="c">Course object that contains the new information</param>
        /// <returns>Json object of true once the course information is saved</returns>
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