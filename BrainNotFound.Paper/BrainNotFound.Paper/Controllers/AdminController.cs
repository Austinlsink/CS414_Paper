﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

//TODO There is a lot to do

namespace BrainNotFound.Paper.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Models.BusinessModels.ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        #region admin controllers
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

        [HttpGet, Route("Settings")]
        public IActionResult Settings()
        {
            return View();
        }
        #endregion admin controllers

        #region instructor controllers
        [HttpGet, Route("Instructors")]
        public async Task<IActionResult> Instructors()
        {
            var allInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).OrderBy(o => o.FirstName).ToList();

            return View(allInstructors);
        }

        [HttpGet, Route("Instructors/New")]
        public IActionResult NewInstructor()
        {
            return View();
        }

        [HttpPost, Route("Instructors/New")]
        public async Task<IActionResult> NewInstructor(ApplicationUser model)
        {
            if (model.FirstName == null || model.LastName == null || model.Password == null)
            {
                return View(model);
            }

            model.UserName = model.FirstName + model.LastName;

            if (await _userManager.FindByNameAsync(model.UserName) == null)
            {
                //Create a new Application User
                var result = await _userManager.CreateAsync(model, model.Password);

                if (result.Succeeded)
                {
                    //Fetch created user
                    var CreatedUser = await _userManager.FindByNameAsync(model.UserName);

                    //Add instructor role to created Application User
                    await _userManager.AddToRoleAsync(CreatedUser, "Instructor");

                    return RedirectToAction("Instructors", "Admin");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ViewData["Message"] += error.Description;
                    }
                }
            }
            else
            {
                ViewBag.UserError = "That user already exists.";
            }
            
            ViewData["message"] += model.Email;
            return View(model);
        }

        [HttpGet, Route("Instructors/{UserName}")]
        public async Task<IActionResult> ViewInstructor(String username)
        {

            var instructor = await _userManager.FindByNameAsync(username);
            instructor.Address = "250 Brent Lane";
            instructor.City = "Pensacola";
            instructor.State = "FL";
            instructor.ZipCode = "32503";

            List<Course> courses = new List<Course>()
            {
                new Course()
                {
                    CourseCode = "CS 306",
                    CourseName = "Database II",
                    CourseId = 1
                },
                new Course()
                {
                    CourseCode = "BI 101",
                    CourseName = "Old Testament Survey",
                    CourseId = 2
                },
                new Course()
                {
                    CourseCode = "EN 126",
                    CourseName = "English Grammar and Composition",
                    CourseId = 3
                }
            };

            List<Section> sections = new List<Section>()
            {
                new Section()
                {
                    CourseId = 1,
                    Capacity = 12,
                    SectionNumber = "1"
                },
                new Section()
                {
                    CourseId = 1,
                    Capacity = 15,
                    SectionNumber = "2"
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 30,
                    SectionNumber = "1"
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 33,
                    SectionNumber = "2"
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 45,
                    SectionNumber = "3"
                },
                new Section()
                {
                    CourseId = 3,
                    Capacity = 32,
                    SectionNumber = "1"
                }

            };

            ViewBag.courses = courses;
            ViewBag.sections = sections;
            ViewBag.profile = instructor;

            return View();
        }

        [HttpGet, Route("Instructors/Edit/{UserName}")]
        public async Task<IActionResult> EditInstructor(String UserName)
        {
            ApplicationUser instructor = await _userManager.FindByNameAsync(UserName);
            ViewBag.instructor = instructor;
            return View();
        }

        [HttpPost, Route("Instructors/Edit/{UserName}")]
        public async Task<IActionResult> EditInstructor(ApplicationUser user)
        {
            var instructor = await _userManager.FindByNameAsync(user.UserName);
            instructor.Salutation  = user.Salutation;
            instructor.FirstName   = user.FirstName;
            instructor.LastName    = user.LastName;
            instructor.PhoneNumber = user.PhoneNumber;
            instructor.Email       = user.Email;
            instructor.Address     = user.Address;
            instructor.City        = user.City;
            instructor.State       = user.State;
            instructor.ZipCode     = user.ZipCode;

            await _userManager.UpdateAsync(instructor);

            ViewData["message"] = user.FirstName;

            return RedirectToAction("Instructors", "Admin");
        }

        ///<summary>
        /// Finds a specified instructor and deletes him from the _userManager - It does work!
        ///</summary>
        ///<param name="UserName">Selected instructor's email</param>
        [HttpDelete("{UserName}"), Route("DeleteInstructor")]
        public async Task<IActionResult> DeleteInstructor(String UserName)
        {
            var instructor = await _userManager.FindByNameAsync(UserName);
            await _userManager.DeleteAsync(instructor);

            return RedirectToAction("Instructors", "Admin");
        }
        #endregion instructor controllers

        #region admin profile controllers
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
        #endregion admin profile controllers

        #region student controllers 
        [HttpGet, Route("Students")]
        public async Task<IActionResult> Students()
        {
            var allStudents = (await _userManager.GetUsersInRoleAsync("Student")).OrderBy(o => o.FirstName).ToList();
            return View(allStudents);
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
        #endregion student controllers 

        #region Department controllers
        [HttpGet, Route("Department/Edit/{Id}")]
        public IActionResult EditDepartment(long Id)
        {
            var department = _context.Departments.Find(Id);

            ViewBag.department = department;
            return View();
        }

        [HttpPost, Route("Department/Edit/{Id}")]
        public IActionResult EditDepartment(Department dept)
        {

            var depart = _context.Departments.Find(dept.DepartmentId);
            depart.DepartmentCode = dept.DepartmentCode;
            depart.DepartmentName = dept.DepartmentName;

            _context.SaveChanges();

            return RedirectToAction("Departments", "Admin");
        }

        // Display the list of departments
        [HttpGet, Route("Departments")]
        public IActionResult Departments()
        {
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();

            return View(departments);
        }

        // Delete a department
        [HttpDelete("{Id:long}"), Route("DeleteDepartment")]
        public IActionResult DeleteDepartment(long id)
        {

            var department = _context.Departments.Find(id);
            _context.Departments.Remove(department);
            _context.SaveChanges();

           return RedirectToAction("Departments", "Admin");
        }

        // Add the details for a new Department
        [HttpGet, Route("Departments/New")]
        public IActionResult NewDepartment()
        {
            return View();
        }

        // Creates the new Department and re-routes to the Department View
        [HttpPost, Route("Departments/New")]
        public IActionResult NewDepartment(Department model)
        {
            if (!ModelState.IsValid)
                return View();

            _context.Departments.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Departments", "Admin");
        }
        #endregion Department controllers


        #region Course controllers

        [HttpGet, Route("Courses")]
        public IActionResult Courses()
        {
            var courses = _context.Courses.OrderBy(o => o.CourseCode).ToList();
            return View(courses);
        }

        [HttpGet, Route("Courses/New")]
        public IActionResult NewCourse()
        {
            return View();
        }

        [HttpPost, Route("Courses/New")]
        public IActionResult NewCourse(Course model)
        {
            if (!ModelState.IsValid)
                return View();

            _context.Courses.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Courses", "Admin");
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

        #endregion course controllers

        // Why do we have a page that lists all sections?, could that be in the courses page?

        #region Section controllers
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
        #endregion section controllers
    }
}
