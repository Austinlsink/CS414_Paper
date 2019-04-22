using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.Models;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper.Services;

namespace BrainNotFound.Paper.Controllers
{
    public class AccountController : Controller
    {
        //Dependencies for Managing users
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;

        // Routes Start

        //[HttpGet, Route("/")]
        public IActionResult Login()
        {
            return View();
        }



        
        //[HttpPost, Route("/")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("LoginReroute", "Account");
                }
            }

            ViewData["Message"] += ModelState.ErrorCount.ToString();
            // If we got this far, something failed, redisplay form
            return View("TestView");
        }

        public ActionResult LoginReroute()
        {

            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("Index", "Instructor");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "Student");
                }
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet, Route("/")]
        public IActionResult ForceLogin()
        {
            return View("LoginOptions");
        }

        public async Task<IActionResult> ForceAdminLogin()
        {
            var result = await _signInManager.PasswordSignInAsync("AbmaelFernandesDaSilva", "admini", false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }
        
        public async Task<IActionResult> ForceInstructorLogin()
        {
            var result = await _signInManager.PasswordSignInAsync("MickeyStemen", "variablesBanana", false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Instructor");
            }

            return View();
        }

        public async Task<IActionResult> ForceStudentLogin()
        {
            //var result = await _signInManager.PasswordSignInAsync("AngellaCetta", "AngellaCetta", false, false);
            var result = await _signInManager.PasswordSignInAsync("BobbyeRhym", "BobbyeRhym", false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Student");
            }

            return View();
        }


        // Web have to change this to a Post verb, but the template must be updated send a post request
        [HttpGet, Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        // Routes End

        // Constructor
        public AccountController(
           SignInManager<ApplicationUser> signInManager,
           UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           PaperDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
    }
}
