using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.DataAccessLayer.Models;
using BrainNotFound.Paper.DataAccessLayer;

namespace BrainNotFound.Paper.WebApp.Controllers
{
    public class RootController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private PaperDbContext _context;

        public RootController(SignInManager<IdentityUser> signInManager, PaperDbContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        [Route("RunCode")]
        public IActionResult Index()
        {
            var d1 = new Department();

            d1.DepartmentCode = "CS";
            d1.DepartmentName = "Computer Science";


            _context.Departments.Add(d1);
            _context.SaveChanges();

            ViewData["Message"] = "Success";

            return View();
        }


        public async Task<IActionResult> ForceLogin()
        {
            var result = await _signInManager.PasswordSignInAsync("AbmaelSilva", "PaperBrain2019!", false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
