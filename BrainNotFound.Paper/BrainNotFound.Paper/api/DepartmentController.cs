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
    [Route("api/Department")]
    [ApiController]
    public class DepartmentController : Controller
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
        public DepartmentController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("New")]
        public IActionResult New([FromBody] Department department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();

            return Json(new { sucess = true });
        }

        [HttpPost, Route("Delete")]
        public IActionResult Delete([FromBody]long DepartmentId)
        {
            var department = _context.Departments.Find(DepartmentId);
            var SuceessMessage = string.Empty;

            if (_context.Courses.Where(ac => ac.DepartmentId == department.DepartmentId).Any())
            {
                var ErrorMessage = department.DepartmentCode + "-" + department.DepartmentName + " could not be deleted. Please delete all associated courses.";
                return Json(new { sucess = false, message = ErrorMessage });
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();
            SuceessMessage = department.DepartmentCode + " " + department.DepartmentName + " was successfully deleted!";


            return Json(new { success = true, message = SuceessMessage });
        }
    }
}