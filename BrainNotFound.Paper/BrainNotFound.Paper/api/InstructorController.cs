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
    [Route("api/Instructor")]
    [ApiController]
    public class InstructorController : Controller
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
        public InstructorController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("Delete")]
        public async Task<IActionResult> Delete([FromBody]string instructorId)
        {
            
            var instructor = await _userManager.FindByIdAsync(instructorId);

            if (_context.Sections.Where(s => s.InstructorId == instructor.Id).Any())
            {
                return Json(new { success = false, message = "Please delete all associated sections before deleting " + instructor.FirstName + " " + instructor.LastName });
            }
            else
            {
                await _userManager.DeleteAsync(instructor);
                return Json(new { success = true, message = "Instructor successfully deleted." });
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
            ApplicationUser instructor = await _userManager.FindByNameAsync(username);

            return Json(new { success = true, firstName = instructor.FirstName,
                                             lastName = instructor.LastName,
                                             salutation = instructor.Salutation,
                                             phone = instructor.PhoneNumber,
                                             email = instructor.Email,
                                             address = instructor.Address,
                                             city = instructor.City,
                                             state = instructor.State,
                                             zip = instructor.ZipCode,
                                             dob = instructor.DOB});
        }

        [HttpPost, Route("SaveChanges")]
        public async Task<IActionResult> SaveChanges([FromBody] ApplicationUser user, string username)
        {
            var updateInstructor = await _userManager.FindByNameAsync(username);
            if (updateInstructor != null)
            {
                updateInstructor.Address = user.Address;
                updateInstructor.State = user.State;
                updateInstructor.City = user.City;
                updateInstructor.ZipCode = user.ZipCode;
                updateInstructor.FirstName = user.FirstName;
                updateInstructor.LastName = user.LastName;
                updateInstructor.UserName = updateInstructor.FirstName + updateInstructor.LastName;
                updateInstructor.PhoneNumber = user.PhoneNumber;
                updateInstructor.Email = user.Email;
                updateInstructor.Salutation = user.Salutation;
                updateInstructor.Password = updateInstructor.Password;

                await _userManager.UpdateAsync(updateInstructor);

                return Json(new { success = true, message = "Instructor successfully updated." });
            }
            else
            {
                return Json(new { success = false, error = "Could not update instructor." });
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
            var instructor = await _userManager.FindByIdAsync(user.Id);

            if (instructor == null)
            {
                //Create a new Application User
                var result = await _userManager.CreateAsync(user, user.Password);
                if (result.Succeeded)
                {
                    //Fetch created user
                    var CreatedUser = await _userManager.FindByNameAsync(user.UserName);

                    //Add Admin role to created Application User
                    await _userManager.AddToRoleAsync(CreatedUser, "Instructor");

                    return Json(new { success = true, message = "Instructor successfully created" });

                }
            }

            return Json(new { success = false, error = "Could not create new instructor." });
        }
    }
}