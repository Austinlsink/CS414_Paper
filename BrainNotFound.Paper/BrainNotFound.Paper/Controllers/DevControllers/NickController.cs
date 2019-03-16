
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.Models;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
using System.IO;
using CsvHelper;
using System.Security.Claims;

namespace BrainNotFound.Paper.Controllers.DevControllers
{
    public class NickController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;
        

        //public IActionResult Run()
        public async Task<IActionResult> Run()
        {

            return View("TestView");
        }

        public async Task<IActionResult> NickSandbox()
        {
            var questions = new List<TrueFalse>();
            var q1 = new TrueFalse()
            {
                Content = "Bima is from Brazil",
                TrueFalseAnswer = true,
                QuestionId = 1
            };

            var q2 = new TrueFalse()
            {
                Content = "Nick is from Brazil",
                TrueFalseAnswer = false,
                QuestionId = 2
            };

            var q3 = new TrueFalse()
            {
                Content = "Austin is from Brazil",
                TrueFalseAnswer = false,
                QuestionId = 3
            };

            questions.Add(q1);
            questions.Add(q2);
            questions.Add(q3);

            ViewBag.Questions = questions;

            return View();
        }
        // Constructor
        public NickController(
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

    }
}
