﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.WebApp.Models.BusinessModels;

//TODO There is a lot to do


namespace BrainNotFound.Paper.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Models.BusinessModels.ApplicationUser> _userManager;
        private readonly PaperDbContext _context;


        // Constructor
        public AdminController(
            UserManager<Models.BusinessModels.ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Start of Routes
        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet, Route("Instructors")]
        public IActionResult Instructors()
        {
            return View();
        }

        [HttpGet, Route("Instructors/New")]
        public IActionResult NewInstructor()
        {
            ViewData["message"] = "ello";

           

            return View();
        }

        [HttpPost, Route("Instructors/New")]
        public IActionResult NewInstructor(CreateInstructorViewModel instructor)
        {
            /*

            //Add the User to the IdentityDbContext
            var result = await _userManager.CreateAsync(newIdentityUser, instructor.Password);

            //Check if user was created successfully
            if (result.Succeeded)
            {
                //Get the user just created
                var NewUserFetched = await _userManager.FindByEmailAsync(newIdentityUser.Email);

                //Populate additional Information
               


                //Add the user Role to the created user
                await _userManager.AddToRoleAsync(NewUserFetched, "Instructor");

                return RedirectToAction("Instructors", "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ViewData["Message"] += error.Description;
                }
            }
*/
            return View("TestView");
            
        }

        [HttpGet, Route("Instructors/{Id}")]
        public IActionResult Instructor(String Id)
        {
            return View("ViewInstructor");
        }

        [HttpGet, Route("Instructors/Edit/{Id}")]
        public IActionResult EditInstructor(String Id)
        {
            return View();
        }

        [HttpGet, Route("Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet, Route("Profile/Edit")]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpGet, Route("Students")]
        public IActionResult Students()
        {
            return View();
        }

        [HttpGet, Route("Students/New")]
        public IActionResult NewStudent()
        {
            return View();
        }

        [HttpGet, Route("Students/{Id}")]
        public IActionResult ViewStudent(String Id)
        {
            return View();
        }

        [HttpGet, Route("Students/Edit/{Id}")]
        public IActionResult EditStudent(String Id)
        {
            return View();
        }

        [HttpGet, Route("Department/Edit/{Id}")]
        public IActionResult EditDepartment(String Id)
        {
            return View();
        }

        // Remove this comment after reading it - Removed Edit Departments - All can be done from this page since there is very little information to deal with
        [HttpGet, Route("Departments")]
        public IActionResult Departments()
        {
            var deparmtents = _context.Departments.ToList();

            return View(deparmtents);
        }

        [HttpGet, Route("Departments/New")]
        public IActionResult NewDepartment()
        {
            return View();
        }

        [HttpPost, Route("Departments/New")]
        public IActionResult NewDepartment(Department model)
        {
            _context.Departments.Add(model);
            _context.SaveChanges();

            ViewData["message"] = "Department code: " + model.DepartmentCode + " " + model.DepartmentName;

            return View();
        }

        [HttpGet, Route("Settings")]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet, Route("Courses")]
        public IActionResult Courses()
        {
            return View();
        }

        [HttpGet, Route("Courses/New")]
        public IActionResult NewCourse()
        {
            return View();
        }

        [HttpGet, Route("Courses/{CourseCode}")]
        public IActionResult ViewCourse(String CourseCode)
        {
            return View();
        }

        [HttpGet, Route("Courses/Edit/{CourseCode}")]
        public IActionResult EditCourse(String CourseCode)
        {
            return View();
        }

        // Why do we have a page that lists all sections?, could that be in the courses page?

        [HttpGet, Route("Sections")]
        public IActionResult Sections()
        {
            return View();
        }

        [HttpGet, Route("Sections/New")]
        public IActionResult NewSection()
        {
            return View();
        }

        [HttpGet, Route("Sections/{CourseCode}/{SectionNumber}")]
        public IActionResult ViewSection(String CourseCode, int SectionNumber)
        {
            return View();
        }

        [HttpGet, Route("Sections/Edit/{CourseCode}/{SectionNumber}")]
        public IActionResult EditSection(String CourseCode, int SectionNumber)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
