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
    public class KaraController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private PaperDbContext _context;




        //public IActionResult Run()
        public async Task<IActionResult> Run()
        {
            // Insert code here

            //foreach (var course in allCourses)
            //{
            //    ViewData["Message"] += "<li>" + course.CourseName + " Sections:";
            //    List<Section> courseSections = new List<Section>();
            //    foreach (var section in allSections)
            //    {
            //        if (section.CourseId == course.CourseId)
            //        {
            //            courseSections.Add(section);
            //            ViewData["Message"] += " " + section.SectionNumber;
            //        }
            //    }
            //    ViewData["Message"] += "</li>";
            //}

            //ViewData["Message"] += "<li>Student Id: " + newEnrollment.StudentId + " Section Id: " + newEnrollment.SectionId + "</li>";

            string instructorName = "Jeff Raplin";
            var allCourses = _context.Courses.OrderBy(o => o.CourseCode).ToList();
            var allDepartments = _context.Departments.OrderBy(o => o.DepartmentCode).ToList();
            var allSections = _context.Sections.OrderBy(o => o.SectionId).ToList();
            var allInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).OrderBy(o => o.FirstName).ToList();

            foreach (var instructor in allInstructors)
            {
                ViewData["Message"] += "<li>Instructor: " + instructor.FullName + "</li>"+ "<li><t/>Sections Taught: ";
                foreach (var section in allSections)
                {
                    if (section.InstructorId == instructor.Id)
                    {
                        //ViewData["Message"] += section.SectionId + " ";
                        foreach (var course in allCourses)
                        {
                            if (section.CourseId == course.CourseId)
                            {
                                foreach (var department in allDepartments)
                                {
                                    if (course.DepartmentId == department.DepartmentId)
                                    {
                                        ViewData["Message"] += department.DepartmentCode + " " + course.CourseName + " " + course.CourseCode + "-";
                                    }
                                }
                                //ViewData["Message"] += "(" + course.CourseName+ "), ";
                            }
                        }
                    }
                }
            }
            ViewData["Message"] += "</li>";

            //_context.SaveChanges();

            return View("TestView");
        }

        // Constructor
        public KaraController(
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
