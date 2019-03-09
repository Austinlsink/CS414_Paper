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

            // Get required data lists
            var allCourses = _context.Courses.OrderBy(o => o.CourseCode).ToList();
            var allSections = _context.Sections.OrderBy(o => o.SectionId).ToList();
            var sectionMeetingTimeList = _context.SectionMeetingTimes.ToList();
            var allStudents = (await _userManager.GetUsersInRoleAsync("Student")).OrderBy(o => o.FirstName).ToList();


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

            // Populate Enrollment table
            foreach (var student in allStudents)
            {
                List<Enrollment> currentEnrollments = new List<Enrollment>(); // Sections the student is already in

                foreach (var enrollment in _context.Enrollments)
                {
                    if (enrollment.StudentId == student.Id)
                    {
                        currentEnrollments.Add(enrollment);
                    }
                }

                // ViewData["Message"] += "<li>" + student.FullName + "</li>";

                // random number generated 4 - 7 for # of sections taken
                int numberOfClasses;
                Random rand = new Random();

                // Get the number of classes the student takes (range: 4-7)
                numberOfClasses = rand.Next(4, 8);

                // Choose 4-7 random courses/sections
                for (int classCounter = 0; classCounter < numberOfClasses; classCounter++)
                {
                    // Get a random course
                    Course selectedCourse = new Course();
                    List<Section> courseSections = new List<Section>();

                    // Get a course that has sections
                    do
                    {
                        selectedCourse = allCourses.ElementAt(rand.Next(0, allCourses.Count));
 
                        // Get all sections related to the selectedCourse
                        foreach (var section in allSections)
                        {
                            if (section.CourseId == selectedCourse.CourseId)
                            {
                                courseSections.Add(section);
                            }
                        }
                    } while (courseSections.Count <= 0);


                    // Select a random section in the selectedCourse
                    Section selectedSection = new Section();
                    bool doesNotConflict = true;
                    bool isNotTaken = true;

                    // Check if a section is already being taken by the student or not
                    do
                    {
                        do
                        {
                            isNotTaken = true;
                            selectedSection = courseSections.ElementAt(rand.Next(0, courseSections.Count));

                            // Check if selectedSection is already being taken
                            foreach (var enrollment in currentEnrollments)
                            {
                                if (selectedSection.SectionId == enrollment.SectionId)
                                {
                                    isNotTaken = false;
                                }
                            }
                        } while (isNotTaken == false);

                        // Make sure section doesn't conflict with other classes
                        doesNotConflict = true;
                        // Check all section meeting times
                        foreach (var sectionMeetingTime in sectionMeetingTimeList)
                        {
                            // The time that the selected section meets
                            if (sectionMeetingTime.SectionId == selectedSection.SectionId)
                            {
                                // If the section meeting time is the same as one of the meeting times for one of the enrollments

                                // Check for conflicts in current enrollments
                                foreach (var enrollment in currentEnrollments)
                                {
                                    foreach (var section in allSections)
                                    {
                                        // Get all sections the student is enrolled in
                                        if (enrollment.SectionId == section.SectionId)
                                        {
                                            foreach (var enrolledSectionSectionMeetingTime in sectionMeetingTimeList)
                                            {
                                                // Check if the section meets at the same time as an already enrolled section
                                                if (section.SectionId == enrolledSectionSectionMeetingTime.SectionId)
                                                {
                                                    if (sectionMeetingTime.SectionMeetingTimeId == enrolledSectionSectionMeetingTime.SectionMeetingTimeId)
                                                    {
                                                        doesNotConflict = false;

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    } while ((isNotTaken == false) || (doesNotConflict == false));
                //} while (isNotTaken == false);

                    // Make a new enrollment using the current student's Id and the SectionId of the selected section
                    Enrollment newEnrollment = new Enrollment
                    {
                        StudentId = student.Id,
                        SectionId = selectedSection.SectionId
                    };
                    _context.Enrollments.Add(newEnrollment);
                    ViewData["Message"] += "<li>Student Id: " + newEnrollment.StudentId + " Section Id: " + newEnrollment.SectionId + "</li>";
                }
                _context.SaveChanges();
            }
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
