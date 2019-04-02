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
    [Route("api/Student")]
    [ApiController]
    public class StudentController : Controller
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
        public StudentController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("Delete")]
        public async Task<IActionResult> Delete([FromBody]string studentId)
        {
            
            var student = await _userManager.FindByIdAsync(studentId);

            if (_context.Enrollments.Where(e => e.StudentId == student.Id).Any())
            {
                return Json(new { success = false, message = "Please remove " + student.FirstName + " " + student.LastName + " from corresponding sections before deleting." });
            }
            else
            {
                await _userManager.DeleteAsync(student);
                return Json(new { success = true, message = "Student successfully deleted." });
            }
        }

        /// <summary>
        /// Allows you to edit an Instructor
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost, Route("Edit/{username}")]
        public async Task<IActionResult> Edit([FromBody]string username)
        {
            ApplicationUser student = await _userManager.FindByNameAsync(username);

            return Json(new { success = true, firstName = student.FirstName,
                                             lastName = student.LastName,
                                             salutation = student.Salutation,
                                             phone = student.PhoneNumber,
                                             email = student.Email,
                                             address = student.Address,
                                             city = student.City,
                                             state = student.State,
                                             zip = student.ZipCode,
                                             dob = student.DOB});
        }

        [HttpPost, Route("SaveChanges")]
        public async Task<IActionResult> SaveChanges([FromBody] ApplicationUser user, string username)
        {
            var updateStudent = await _userManager.FindByNameAsync(username);
            if (updateStudent != null)
            {
                updateStudent.Address = user.Address;
                updateStudent.State = user.State;
                updateStudent.City = user.City;
                updateStudent.ZipCode = user.ZipCode;
                updateStudent.FirstName = user.FirstName;
                updateStudent.LastName = user.LastName;
                updateStudent.UserName = updateStudent.FirstName + updateStudent.LastName;
                updateStudent.PhoneNumber = user.PhoneNumber;
                updateStudent.Email = user.Email;
                updateStudent.Salutation = user.Salutation;
                updateStudent.Password = updateStudent.Password;

                await _userManager.UpdateAsync(updateStudent);

                return Json(new { success = true, message = "Student successfully updated." });
            }
            else
            {
                return Json(new { success = false, error = "Could not update student." });
            }
        }

        /// <summary>
        /// Create a new Instructor
        /// </summary>
        /// <param name="user">Instructor information</param>
        /// <returns></returns>
        [HttpPost, Route("New")]
        public async Task<IActionResult> New([FromBody] ApplicationUser user)
        {
            user.UserName = user.FirstName + user.LastName;
            var student = await _userManager.FindByIdAsync(user.Id);

            if (student == null)
            {
                //Create a new Application User
                var result = await _userManager.CreateAsync(user, user.Password);
                if (result.Succeeded)
                {
                    //Fetch created user
                    var CreatedUser = await _userManager.FindByNameAsync(user.UserName);

                    //Add Admin role to created Application User
                    await _userManager.AddToRoleAsync(CreatedUser, "Student");

                    return Json(new { success = true, message = "Student successfully created" });

                }
            }

            return Json(new { success = false, error = "Could not create new student." });
        }
    }
}