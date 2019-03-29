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

        [HttpPost, Route("New")]
        public async Task<IActionResult> New([FromBody] JsonObject jsonData)
        {
            dynamic user = jsonData;
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

                    return Json(new { success = true });

                }
            }

            return Json(new { success = false });
        }
    }
}