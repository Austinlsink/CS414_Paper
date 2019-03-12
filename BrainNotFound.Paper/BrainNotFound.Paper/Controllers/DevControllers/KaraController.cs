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

            string instructorName = "Mike Redhead";


            List<Course> coursesTaught = new List<Course>();
            var allCourses = _context.Courses.OrderBy(o => o.CourseCode).ToList();
            var allDepartments = _context.Departments.OrderBy(o => o.DepartmentCode).ToList();
            var allSections = _context.Sections.OrderBy(o => o.SectionId).ToList();
            var allInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).OrderBy(o => o.FirstName).ToList();

            foreach (var instructor in allInstructors)
            {
                if (instructor.FullName == instructorName)
                {
                    ViewData["Message"] += "<li>" + instructorName + " teaches: </li>";
                    foreach (var section in allSections)
                    {
                        if (section.InstructorId == instructor.Id)
                        {
                            foreach (var course in allCourses)
                            {
                                if (section.CourseId == course.CourseId)
                                {
                                    if (coursesTaught.Contains(course) == false)
                                    {
                                        coursesTaught.Add(course);
                                        ViewData["Message"] += "<li><t/> " + course.CourseName + "</li>";
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return View("TestView");
        }

        public async Task<IActionResult> FindCoursesTaughtBy()
        {
            string instructorName = "Mike Redhead";
            List<Course> coursesTaught = new List<Course>();       
            var instructor = _context.ApplicationUsers.Where(AU => AU.FullName == instructorName).First();
            var sectionsTaught = _context.Sections.Where(S => S.InstructorId == instructor.Id);

            foreach (var section in sectionsTaught)
            {
                var currentCourse = _context.Courses.Where(c => c.CourseId == section.CourseId).First();
                    if (coursesTaught.Contains(currentCourse) == false)
                    {
                        coursesTaught.Add(currentCourse);
                        ViewData["Message"] += "<li><t/> " + currentCourse.CourseName + "</li>";
                    }
            }

            return View("TestView");
        }

        public async Task<IActionResult> GetStudentsInSection()
        {
            long sectionNumber = 201;
            List<ApplicationUser> studentsIn = new List<ApplicationUser>();       
            var section = _context.Sections.Where(S => S.SectionId == sectionNumber).First();
            var enrollments = _context.Enrollments.Where(E => E.SectionId == sectionNumber);

            foreach (var enrollment in enrollments)
            {
                var currentStudent = _context.ApplicationUsers.Where(AU => AU.Id == enrollment.StudentId).First();
                studentsIn.Add(currentStudent);
                ViewData["Message"] += "<li> " + currentStudent.FullName + "</li>";
            }

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
