using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.WebApp.Models.BusinessModels;

namespace BrainNotFound.Paper.WebApp.Controllers
{
    public class RootController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private PaperDbContext _context;

        public RootController(
            SignInManager<ApplicationUser> signInManager, 
            PaperDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Route("RunCode")]
        public async Task<IActionResult> Index()
        {
            /*var d1 = new Department();

            d1.DepartmentCode = "CS";
            d1.DepartmentName = "Computer Science";


            _context.Departments.Add(d1);
            _context.SaveChanges();

            ViewData["Message"] = "Success";
            


            // Created the Roles in the Database
            string[] roleNames = { "Admin", "Instructor", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                     await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            */

            var user = await _userManager.GetUserAsync(User);

            await _userManager.AddToRoleAsync(user, "Admin");
            return View();
        }

        [Route("/")]
        public async Task<IActionResult> ForceLogin()
        {
            var result = await _signInManager.PasswordSignInAsync("AbmaelSilva", "PaperBrain2019!", false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        
    }
}
