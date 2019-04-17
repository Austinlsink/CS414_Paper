using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
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
        public InstructorController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        /// <summary>
        /// Allows the admin to delete an instructor as long as the instructor is note assigned to any sections
        /// </summary>
        /// <param name="instructorId">Search criteria for finding the specific instructor</param>
        /// <returns>Json result of either true if deleting the instructor is successful; otherwise, false</returns>
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
        /// Allows the admin to edit an instructor's information
        /// </summary>
        /// <param name="username">Search criteria for finding the specific instructor</param>
        /// <returns>Json result of true and the instructor's profile information</returns>
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

        /// <summary>
        /// Allows the admin to save the edited changes to an instructor
        /// </summary>
        /// <param name="user">ApplicationUser object that contains the new information</param>
        /// <param name="username">Saerch criteria for the specific instructor</param>
        /// <returns>Json result of either true if an instructor was successfully updated; otherwise, false</returns>
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
        /// Allows the admin to create a new instructor
        /// </summary>
        /// <param name="user">ApplicationUser object that contains all of the needed information</param>
        /// <returns>Json result of either true if the instructor is successfully created; otherwise, false</returns>
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