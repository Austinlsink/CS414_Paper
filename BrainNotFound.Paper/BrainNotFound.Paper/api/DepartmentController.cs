using System.Linq;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
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
        public DepartmentController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        /// <summary>
        /// Allows the admin to create a new department
        /// </summary>
        /// <param name="department">Department object that contains all of the information for a new department</param>
        /// <returns>Json result of true</returns>
        [HttpPost, Route("New")]
        public IActionResult New([FromBody] Department department)
        {
            var departmentLookUpByDC = _context.Departments.Where(d => d.DepartmentCode == department.DepartmentCode).FirstOrDefault();
            var departmentLookUpByDN = _context.Departments.Where(d =>  d.DepartmentName == department.DepartmentName).FirstOrDefault();

            if (departmentLookUpByDC != null || departmentLookUpByDN != null)
            {
                string DeparmentCodeError = "";
                string DeparmentNameError = "";

                if (departmentLookUpByDC != null)
                    DeparmentCodeError = "Department code must be unique";

                if (departmentLookUpByDN != null)
                    DeparmentNameError = "Department name must be unique";


                return Json(new { sucess = false, DeparmentCodeError, DeparmentNameError });
            }

            _context.Departments.Add(department);
            _context.SaveChanges();
            return Json(new { sucess = true });
        }

        /// <summary>
        /// Allows the admin to delete a department if there are no associated courses with it
        /// </summary>
        /// <param name="DepartmentId">Search criteria for finding the specific department</param>
        /// <returns>Json result of either true if the department was successfully deleted; otherwise, false</returns>
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