using System;
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
using Microsoft.AspNetCore.Http;
using BrainNotFound.Paper.Models.ViewModels;

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

        #region create administrator controllers
        [HttpGet, Route("Administrators")]
        public async Task<IActionResult> Administrators()
        {
            var allAdministrators = (await _userManager.GetUsersInRoleAsync("Admin")).OrderBy(o => o.FirstName).ToList();

            return View(allAdministrators);
        }

        [HttpGet, Route("Administrators/New")]
        public IActionResult NewAdministrator()
        {
            return View();
        }

        [HttpPost, Route("Administrators/New")]
        public async Task<IActionResult> NewAdministrator(ApplicationUser model)
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
                    await _userManager.AddToRoleAsync(CreatedUser, "Admin");

                    return RedirectToAction("Administrators", "Admin");
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

        [HttpGet, Route("Administrators/{UserName}")]
        public async Task<IActionResult> ViewAdministrator(String username)
        {

            var admin = await _userManager.FindByNameAsync(username);

            ViewBag.profile = admin;

            return View();
        }

        [HttpGet, Route("Administrators/Edit/{UserName}")]
        public async Task<IActionResult> EditAdministrator(String UserName)
        {
            ApplicationUser admin = await _userManager.FindByNameAsync(UserName);
            ViewBag.admin = admin;
            return View();
        }

        [HttpPost, Route("Administrators/Edit/{UserName}")]
        public async Task<IActionResult> EditAdministrator(ApplicationUser user)
        {
            var admin = await _userManager.FindByNameAsync(user.UserName);
            admin.Salutation = user.Salutation;
            admin.FirstName = user.FirstName;
            admin.LastName = user.LastName;
            admin.PhoneNumber = user.PhoneNumber;
            admin.Email = user.Email;
            admin.Address = user.Address;
            admin.City = user.City;
            admin.State = user.State;
            admin.ZipCode = user.ZipCode;

            await _userManager.UpdateAsync(admin);

            ViewData["message"] = user.FirstName;

            return RedirectToAction("Administrators", "Admin");
        }

        ///<summary>
        /// Finds a specified instructor and deletes him from the _userManager - It does work!
        ///</summary>
        ///<param name="UserName">Selected instructor's email</param>
        [HttpDelete("{UserName}"), Route("DeleteAdministrator")]
        public async Task<IActionResult> DeleteAdministrator(String UserName)
        {
            var admin = await _userManager.FindByNameAsync(UserName);
            await _userManager.DeleteAsync(admin);

            return RedirectToAction("Administrators", "Admin");
        }
        #endregion create administrator controllers

        #region create instructor controllers
        [HttpGet, Route("Instructors")]
        public async Task<IActionResult> Instructors(String message = "")
        {
            var allInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).OrderBy(o => o.FirstName).ToList();
            ViewBag.Message = message;
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
                    SectionNumber = 1
                },
                new Section()
                {
                    CourseId = 1,
                    Capacity = 15,
                    SectionNumber = 2
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 30,
                    SectionNumber = 1
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 33,
                    SectionNumber = 2
                },
                new Section()
                {
                    CourseId = 2,
                    Capacity = 45,
                    SectionNumber = 3
                },
                new Section()
                {
                    CourseId = 3,
                    Capacity = 32,
                    SectionNumber = 1
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
        ///<param name="user">Selected instructor's email</param>
        [HttpPost, Route("DeleteInstructor")]
        public async Task<IActionResult> DeleteInstructor(Delete deleteUser)
        {
            var instructor = await _userManager.FindByNameAsync(deleteUser.UserName);
            if(_context.Sections.Where(s => s.InstructorId == instructor.Id).Any())
            {
                return RedirectToAction("Instructors", "Admin", new { message = "Error: Please delete all associated sections before deleting " + instructor.FirstName + " " + instructor.LastName });
            }
            else
            {
                await _userManager.DeleteAsync(instructor);
            }

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

        [HttpPost, Route("Students/New")]
        public async Task<IActionResult> NewStudent(ApplicationUser model)
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
                    await _userManager.AddToRoleAsync(CreatedUser, "Student");

                    return RedirectToAction("Students", "Admin");
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

        [HttpGet, Route("Students/{UserName}")]
        public async Task<IActionResult> ViewStudent(String username)
        {

            var student = await _userManager.FindByNameAsync(username);

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

            ViewBag.courses = courses;
            ViewBag.profile = student;

            return View();
        }

        [HttpGet, Route("Students/Edit/{UserName}")]
        public async Task<IActionResult> EditStudent(String UserName)
        {
            ApplicationUser student = await _userManager.FindByNameAsync(UserName);
            ViewBag.student = student;
            return View();
        }

        [HttpPost, Route("Students/Edit/{UserName}")]
        public async Task<IActionResult> EditStudent(ApplicationUser user)
        {
            var student = await _userManager.FindByNameAsync(user.UserName);
            student.Salutation  = user.Salutation;
            student.FirstName   = user.FirstName;
            student.LastName    = user.LastName;
            student.PhoneNumber = user.PhoneNumber;
            student.Email       = user.Email;
            student.Address     = user.Address;
            student.City        = user.City;
            student.State       = user.State;
            student.ZipCode     = user.ZipCode;

            await _userManager.UpdateAsync(student);

            ViewData["message"] = user.FirstName;

            return RedirectToAction("Students", "Admin");
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
        public IActionResult Departments(string message = "")
        {
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();
            var courses = _context.Courses.ToList();
            ViewBag.courses = courses;
            ViewBag.Message = message;
            return View(departments);
        }

        // Delete a department
        [HttpPost, Route("DeleteDepartment")]
        public IActionResult DeleteDepartment(Delete deleteDepartment)
        {
            var department = _context.Departments.Find(deleteDepartment.DepartmentId);

            if (_context.Courses.Where(ac => ac.DepartmentId == department.DepartmentId).Any())
            {
               return RedirectToAction("Departments", "Admin", new { message = "Error: Please delete all associated courses before deleting " + department.DepartmentCode + " " + department.DepartmentName });
            }
            else
            {
                _context.Departments.Remove(department);
                _context.SaveChanges();
            }
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
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();
            ViewBag.departmentList = departments;
            return View(courses);
        }

        [HttpGet, Route("Courses/New")]
        public IActionResult NewCourse()
        {
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();
            ViewBag.departmentList = departments;

            return View();
        }

        [HttpPost, Route("Courses/New")]
        public IActionResult NewCourse(Course model)
        {
            if (!ModelState.IsValid)
                return View();

            var department = _context.Departments.Find(model.DepartmentId);
            model.DepartmentId = department.DepartmentId;
            model.DepartmentCode = department.DepartmentCode;

            _context.Courses.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Courses", "Admin");
        }

        [HttpGet, Route("Courses/{id}")]
        public async Task<IActionResult> ViewCourse(long id)
        {
            // Find the specified course and add it to the ViewBag
            var course = _context.Courses.Find(id);
            ViewBag.course = course;

            // Find the list of departments and add it to the ViewBag
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();
            ViewBag.departmentList = departments;

            // Find the sections with the same ID as the course and add it to the ViewBag
            var sections = _context.Sections.Where(s => s.CourseId == course.CourseId);
            ViewBag.sectionsList = sections;

            // Get all of the instructors and add them to the ViewBag
            var allInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).OrderBy(o => o.FirstName).ToList();
            ViewBag.instructorList = allInstructors;

            // Get all of the SectionMeetingTimes and add them to the ViewBag
            var allSectionMeetingTimes = _context.SectionMeetingTimes.ToList();
            ViewBag.sectionMeetingTimeList = allSectionMeetingTimes;

            return View();
        }

        [HttpGet, Route("Courses/Edit/{id}")]
        public IActionResult EditCourse(long Id)
        {
            var course = _context.Courses.Find(Id);
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();
            ViewBag.course = course;
            ViewBag.departments = departments;
            return View();
        }

        [HttpPost, Route("Courses/Edit/{id}")]
        public IActionResult EditCourse(Course c)
        {

            var course = _context.Courses.Find(c.CourseId);
            course.CourseCode  = c.CourseCode;
            course.CourseName  = c.CourseName;
            course.Description = c.Description;
            course.CreditHours = c.CreditHours;

            _context.SaveChanges();

            return RedirectToAction("Courses", "Admin");
        }

        // Delete a Course
        [HttpDelete("{id:long}"), Route("DeleteCourse")]
        public IActionResult DeleteCourse(long id)
        {
            var course = _context.Courses.Find(id);
            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("Courses", "Admin");
        }

        #endregion course controllers

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
