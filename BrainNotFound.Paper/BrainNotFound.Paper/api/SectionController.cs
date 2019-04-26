using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BrainNotFound.Paper.api
{
    [Authorize(Roles = "Admin")]
    [Route("api/Admin")]
    [ApiController]
    public class SectionController : Controller
    {
        #region Initialize controllers
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        public SectionController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        /// <summary>
        /// Allows the admin to delete a section
        /// </summary>
        /// <param name="adminId">Search criteria for the specific admin to remove</param>
        /// <returns>Json object of either true if deletion is successful or false</returns>
        [HttpPost, Route("Delete")]
        public JsonResult Delete([FromBody] JObject JsonData)
        {
            dynamic section = JsonData;

            return Json(new { sucess = true });
        }
    }
}