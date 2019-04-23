using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BrainNotFound.Paper.api
{
    [Authorize(Roles = "Instructor")]
    [Route("api/Instructor")]
    [ApiController]
    public class IndexController : Controller
    {
        #region Initialize controllers
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        public IndexController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

      [HttpPost, Route("DisplayTestCharts")]
        public JsonResult DisplayTestCharts([FromBody] long id)
        {
            return Json(new { success = true });
        }
    }
}