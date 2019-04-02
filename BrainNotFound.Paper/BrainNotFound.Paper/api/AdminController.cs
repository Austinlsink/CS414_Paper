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
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : Controller
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
        public AdminController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("Delete")]
        public async Task<IActionResult> Delete([FromBody]string adminId)
        {
            
            var admin = await _userManager.FindByIdAsync(adminId);
            if (admin.UserName != User.Identity.Name)
            {
                await _userManager.DeleteAsync(admin);
                return Json(new { success = true, message = "Admin successfully deleted." });
            }
            else
            {
                return Json(new { success = false, message = "Sorry, but you can't remove yourself." });

            }
        }

        /// <summary>
        /// Allows you to edit an administrator
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost, Route("Edit/{username}")]
        public async Task<IActionResult> Edit([FromBody]string username)
        {
            ApplicationUser admin = await _userManager.FindByNameAsync(username);

            return Json(new { success = true, firstName = admin.FirstName,
                                             lastName = admin.LastName,
                                             salutation = admin.Salutation,
                                             phone = admin.PhoneNumber,
                                             email = admin.Email,
                                             address = admin.Address,
                                             city = admin.City,
                                             state = admin.State,
                                             zip = admin.ZipCode,
                                             dob = admin.DOB});
        }

        [HttpPost, Route("SaveChanges")]
        public async Task<IActionResult> SaveChanges([FromBody] ApplicationUser user, string username)
        {
            var updateAdmin = await _userManager.FindByNameAsync(username);
            if (updateAdmin != null)
            {
                updateAdmin.Address = user.Address;
                updateAdmin.State = user.State;
                updateAdmin.City = user.City;
                updateAdmin.ZipCode = user.ZipCode;
                updateAdmin.FirstName = user.FirstName;
                updateAdmin.LastName = user.LastName;
                updateAdmin.UserName = updateAdmin.FirstName + updateAdmin.LastName;
                updateAdmin.PhoneNumber = user.PhoneNumber;
                updateAdmin.Email = user.Email;
                updateAdmin.Salutation = user.Salutation;
                updateAdmin.Password = updateAdmin.Password;

                await _userManager.UpdateAsync(updateAdmin);

                return Json(new { success = true, message = "Admin successfully updated." });
            }
            else
            {
                return Json(new { success = false, error = "Could not update admin." });
            }
        }

        /// <summary>
        /// Create a new administrator
        /// </summary>
        /// <param name="user">Admin information</param>
        /// <returns></returns>
        [HttpPost, Route("New")]
        public async Task<IActionResult> New([FromBody] ApplicationUser user)
        {
            user.UserName = user.FirstName + user.LastName;
            var admin = await _userManager.FindByIdAsync(user.Id);

            if (admin == null)
            {
                //Create a new Application User
                var result = await _userManager.CreateAsync(user, user.Password);
                if (result.Succeeded)
                {
                    //Fetch created user
                    var CreatedUser = await _userManager.FindByNameAsync(user.UserName);

                    //Add Admin role to created Application User
                    await _userManager.AddToRoleAsync(CreatedUser, "Admin");

                    return Json(new { success = true, message = "Admin successfully created" });

                }
            }

            return Json(new { success = false, error = "Could not create new admin." });
        }
    }
}