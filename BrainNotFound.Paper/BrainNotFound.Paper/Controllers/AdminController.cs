using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;

        #region admin controllers

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        public AdminController(
            UserManager<ApplicationUser> userManager, PaperDbContext context)
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
        /// <summary>
        /// Returns a list of all the administrators
        /// </summary>
        [HttpGet, Route("Administrators")]
        public async Task<IActionResult> Administrators()
        {
            var allAdministrators = (await _userManager.GetUsersInRoleAsync("Admin")).OrderBy(o => o.FirstName).ToList();

            return View(allAdministrators);
        }

        /// <summary>
        /// GET: Displays the form for a new administrator
        /// </summary>
        [HttpGet, Route("Administrators/New")]
        public IActionResult NewAdministrator()
        {
            return View();
        }

        /// <summary>
        /// POST: Creates a new administrator profile
        /// </summary>
        /// <param name="model">The admin info that is being added to the _userManager</param>
        [HttpPost, Route("Administrators/New")]
        public async Task<IActionResult> NewAdministrator(ApplicationUser model)
        {
            if (model.FirstName == null || model.LastName == null || model.Password == null)
            {
                return View(model);
            }

            model.UserName = model.FirstName + model.LastName;
            var admin = await _userManager.FindByIdAsync(model.Id);

            if (admin == null)
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
                        ViewBag.UserError += error.Description;
                    }
                }
            }
            else
            {
                ViewBag.UserError = "That user already exists.";
                ViewData["Message"] = admin.Id.ToString();
                return View("TestView");
            }
            ViewData["message"] += model.Email;
            return View(model);
        }

        /// <summary>
        /// Views a specific administrator's information
        /// </summary>
        /// <param name="username">The specific admin to look up</param>
        [HttpGet, Route("Administrators/{UserName}")]
        public async Task<IActionResult> ViewAdministrator(String username)
        {
            var admin = await _userManager.FindByNameAsync(username);

            return View(admin);
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
        [HttpPost, Route("DeleteAdministrator")]
        public async Task<IActionResult> DeleteAdministrator(String UserName)
        {
            var admin = await _userManager.FindByNameAsync(UserName);
            await _userManager.DeleteAsync(admin);

            return RedirectToAction("Administrators", "Admin");
        }
        #endregion create administrator controllers

        #region create instructor controllers
        /// <summary>
        /// Allows the user to view all of the instructors
        /// </summary>
        /// <param name="message">Error message from the DeleteInstructor method.</param>
        /// <returns></returns>
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
            // Find the specified user by his username and add him to the ViewBag
            var instructor = await _userManager.FindByNameAsync(username);
            ViewBag.profile = instructor;

            // Grab all of the departments and add them to the ViewBag
            List<Department> departments = _context.Departments.ToList();
            ViewBag.departments = departments;

            // Find all of the sections that the instructor teaches and add them to the ViewBag
            List<Section> sections = _context.Sections.Where(s => s.InstructorId == instructor.Id).ToList();
            ViewBag.sections = sections;

            // Find all of the courses that match its corresponding section and add them to the ViewBag
            List<Course> allCourses = _context.Courses.ToList();
            List<Course> courses = new List<Course>();
            foreach (Section s in sections)
            {
                foreach (Course c in allCourses)
                {
                    if (s.CourseId == c.CourseId)
                        if (!courses.Contains(c))
                            courses.Add(c);
                }
            }

            ViewBag.courses = courses;

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
            instructor.Salutation = user.Salutation;
            instructor.FirstName = user.FirstName;
            instructor.LastName = user.LastName;
            instructor.PhoneNumber = user.PhoneNumber;
            instructor.Email = user.Email;
            instructor.Address = user.Address;
            instructor.City = user.City;
            instructor.State = user.State;
            instructor.ZipCode = user.ZipCode;

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
            if (_context.Sections.Where(s => s.InstructorId == instructor.Id).Any())
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
        public async Task<IActionResult> Profile()
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.profile = admin;
            return View();
        }

        [HttpGet, Route("Profile/Edit")]
        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser admin = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.admin = admin;

            return View();
        }

        [HttpPost, Route("Profile/Edit")]
        public async Task<IActionResult> EditProfile(ApplicationUser user)
        {
            ApplicationUser admin = await _userManager.GetUserAsync(HttpContext.User);
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
            return RedirectToAction("Profile", "Admin");
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
            var enrollment = _context.Enrollments.Where(e => e.StudentId == student.Id).ToList();
            var allSections = _context.Sections.ToList();
            var allMeetingTimes = _context.SectionMeetingTimes.ToList();
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();
            List<Section> sections = new List<Section>();

            foreach (Enrollment e in enrollment)
            {
                foreach (Section s in allSections)
                {
                    if (e.SectionId == s.SectionId)
                    {
                        sections.Add(s);
                    }
                }
            }

            ViewBag.profile = student;
            ViewBag.sections = sections;
            ViewBag.sectionMeetingTimesList = allMeetingTimes;
            ViewBag.courses = courses;
            ViewBag.departments = departments;

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
            student.Salutation = user.Salutation;
            student.FirstName = user.FirstName;
            student.LastName = user.LastName;
            student.PhoneNumber = user.PhoneNumber;
            student.Email = user.Email;
            student.Address = user.Address;
            student.City = user.City;
            student.State = user.State;
            student.ZipCode = user.ZipCode;

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
        [HttpGet, Route("departments")]
        public IActionResult Departments(String message)
        {
            var courses = _context.Courses.ToList();
            var departments = _context.Departments.ToList();
            //ViewBag.deleteMessage = message;
            //ViewBag.confirmMessage = message

            if (message != null)
            {
                if (message.StartsWith("Error"))
                {
                    ViewBag.deleteMessage = message;
                }
                else
                {
                    ViewBag.confirmMessage = message;
                }
            }
            
            ViewBag.courses = courses;
            return View(departments.ToList());
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
            return RedirectToAction("Departments", "Admin", new { message = department.DepartmentCode + " " + department.DepartmentName + " was successfully deleted!" });
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
        public IActionResult Courses(String message = "")
        {
            var courses = _context.Courses.OrderBy(o => o.CourseCode).ToList();
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();
            ViewBag.departmentList = departments;
            ViewBag.Message = message;
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
        public IActionResult NewCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();
                ViewBag.departmentList = departments;
                return View();
            }

            Department department = _context.Departments.Find(course.DepartmentId);
            course.Department = department;

            _context.Courses.Add(course);
            _context.SaveChanges();
            return RedirectToAction("Courses", "Admin");
        }

        /// <summary>
        /// Allows the user to view a specific course and all of its information
        /// </summary>
        /// <param name="code">The DepartmentCode and CourseCode</param>
        /// <returns></returns>
        [HttpGet, Route("Courses/{code}")] // ex: SP101
        public async Task<IActionResult> ViewCourse(string code)
        {
            // Parsing code into the DepartmentCode and the CourseCode
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

            // Find the department and add it to the ViewBag
            var department = _context.Departments.Where(d => d.DepartmentCode == departmentCode).First();
            ViewBag.department = department;

            // Find the specified course and add it to the ViewBag
            var course = _context.Courses.Where(c => c.CourseCode == courseCode && c.DepartmentId == department.DepartmentId).First();
            ViewBag.course = course;

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
            ViewData["description"] = course.Description;
            return View();
        }

        [HttpPost, Route("Courses/Edit/{id}")]
        public IActionResult EditCourse(Course c, string description)
        {
            var course = _context.Courses.Find(c.CourseId);
            course.CourseCode  = c.CourseCode;
            course.Name  = c.Name;
            course.Description = description;
            course.CreditHours = c.CreditHours;
            _context.Courses.Update(course);

            _context.SaveChanges();

            return RedirectToAction("Courses", "Admin");
        }

        // Delete a Course
        [HttpPost, Route("DeleteCourse")]
        public IActionResult DeleteCourse(Delete deleteCourse)
        {
            var course = _context.Courses.Find(deleteCourse.CourseId);

            if (_context.Sections.Where(ac => ac.CourseId == course.CourseId).Any())
            {
                return RedirectToAction("Courses", "Admin", new { message = "Error: Please delete all associated sections before deleting " + course.DepartmentCode });
            }
            else
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Courses", "Admin");
        }

        #endregion course controllers

        #region Section controllers
        [HttpGet, Route("Sections")]
        public IActionResult Sections()
        {
            return View();
        }

        [HttpGet, Route("Sections/New/{code}")]
        public async Task<IActionResult> NewSection(string code)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode     = code.Substring(2, 3);

            // Find the department associated with the course by DepartmentCode and add it to the ViewBag
            var department = _context.Departments.Where(d => d.DepartmentCode == departmentCode).First();
            ViewBag.department = department;

            // Find the section's course where the CourseCode and DepartmentIds match and add it to the ViewBag 
            var course = _context.Courses.Where(c => c.CourseCode == courseCode && c.DepartmentId == department.DepartmentId).First();
            ViewBag.course = course;

            // Find all of the instructors and add them to the ViewBag
            var instructor = await _userManager.GetUsersInRoleAsync("Instructor");
            ViewBag.instructorList = instructor;

            SectionMeetingTime sectionMeeting = new SectionMeetingTime();
            sectionMeeting.Day = "Monday Tuesday Wednesday Thursday Friday";
            ViewBag.sectionMeetingDays = sectionMeeting;

            // Generate a section number
            var allSectionForCourse = _context.Sections.Where(s => s.CourseId == course.CourseId);

            int sectionNumber = 0;
            IQueryable<Section> SectionswithId;

            bool SectionNumberFound = false;
            do
            {
                sectionNumber += 1;
                SectionswithId = allSectionForCourse.Where(asfc => asfc.SectionNumber == sectionNumber);
                SectionNumberFound = SectionswithId.Any();

            } while (SectionNumberFound);

            ViewBag.sectionNumber = sectionNumber;
            
            return View();
        }

        [HttpPost, Route("Sections/New/{code}")]
        public async Task<IActionResult> NewSection(String code, Section section, string[] daysMet, DateTime startTime, DateTime endTime)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode     = code.Substring(2, 3);

            if(section.InstructorId == null || startTime == null || endTime == null)
            {
                return View(code);
            }

            // Find the department and course that are associated with the section
            Department department = _context.Departments.Where(d => d.DepartmentCode == departmentCode).First();
            Course course = _context.Courses.Where(c => c.CourseCode == courseCode && c.DepartmentId == department.DepartmentId).First();
 
            // Create the new section meeting time list and add it to the new section
            List<SectionMeetingTime> allDaysMet = new List<SectionMeetingTime>();

            foreach (String s in daysMet)
            {
                SectionMeetingTime newSectionMeetingTime = new SectionMeetingTime();
                newSectionMeetingTime.Day = s;
                newSectionMeetingTime.StartTime = startTime;
                newSectionMeetingTime.EndTime = endTime;
                newSectionMeetingTime.Section = section;

                allDaysMet.Add(newSectionMeetingTime);
            }

            // Create the new section
            Section newSection = new Section();
            newSection.Location = section.Location;
            newSection.Capacity = section.Capacity;
            newSection.SectionNumber = section.SectionNumber;
            newSection.Course = course;
            newSection.ApplicationUser = await _userManager.FindByIdAsync(section.InstructorId);
            newSection.InstructorId = section.InstructorId;
            newSection.SectionNumber = section.SectionNumber;
            newSection.TimesMet = allDaysMet;

            _context.Sections.Add(newSection);
            _context.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        /// <summary>
        /// Allows the user to view a specific section
        /// </summary>
        /// <param name="code">DepartmentCode + CourseCode</param>
        /// <param name="sectionNumber">section number for the corresonding course</param>
        /// <returns></returns>
        [HttpGet, Route("Sections/View/{code}/{sectionNumber}")]
        public async Task<IActionResult> ViewSection(string code, int sectionNumber)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode     = code.Substring(2, 3);

            // Find the department associated with the course by DepartmentCode and add it to the ViewBag
            var department = _context.Departments.Where(d => d.DepartmentCode == departmentCode).First();
            ViewBag.department = department;

            // Find the section's course where the CourseCode and DepartmentIds match and add it to the ViewBag 
            var course = _context.Courses.Where(c => c.CourseCode == courseCode && c.DepartmentId == department.DepartmentId).First();
            ViewBag.course = course;

            // Find the section and its information where the CourseIds and SectionNumber match and add it to the ViewBag
            var section = _context.Sections.Where(s => s.CourseId == course.CourseId && s.SectionNumber == sectionNumber).First();
            ViewBag.section = section;

            // Find all of the instructors and add them to the ViewBag
            var instructor = await _userManager.GetUsersInRoleAsync("Instructor");
            ViewBag.instructorList = instructor;

            // Find all of the students and add them to the ViewBag
            var students = await _userManager.GetUsersInRoleAsync("Student");
            ViewBag.students = students;

            // Find all enrollments and add them to the ViewBag
            var enrollment = _context.Enrollments.ToList();
            ViewBag.enrollment = enrollment;

            // Find all the SectionMeetingTimes and add them to the ViewBag
            var sectionMeetingTimeList = _context.SectionMeetingTimes.ToList();
            ViewBag.sectionMeetingTimeList = sectionMeetingTimeList;

            return View();
        }

        /// <summary>
        /// Allows the user to reassign an instructor to a section
        /// </summary>
        /// <param name="user">Requested instructor to assign to the section</param>
        /// <param name="section">The section being reassigned a new instructor</param>
        /// <param name="course"></param>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost, Route("ReassignInstructor")]
        public async Task<IActionResult> ReassignInstructor(ApplicationUser user, Section section, Course course, Department department)
        {
            string code = department.DepartmentCode + course.CourseCode;
            //ViewData["message"] = code + "Hello, we made it...";
            //return View("TestView");

            // Find the instructor to reassign to the specified section
            var instructor = await _userManager.FindByNameAsync(user.UserName);

            // Find the specified section to reassign the instructor
            var sect = _context.Sections.Where(s => s.SectionId == section.SectionId).First();
            sect.InstructorId = instructor.Id;

            _context.SaveChanges();    

            return RedirectToAction("ViewSection", "Admin", new { code, section.SectionNumber });
        }

        // Assign a student to a section
        [HttpPost, Route("AssignStudent")]
        public async Task<IActionResult> AssignStudent(ApplicationUser user, Section section, Course course, Department department)
        {
            string code = department.DepartmentCode + course.CourseCode;

            var student = await _userManager.FindByNameAsync(user.UserName);

            Enrollment enroll = new Enrollment();
            enroll.SectionId = section.SectionId;
            enroll.StudentId = student.Id;

            _context.Enrollments.Add(enroll);
            _context.SaveChanges();
           
            return RedirectToAction("ViewSection", "Admin", new { code, section.SectionNumber });
        }

        [HttpPost, Route("UnassignStudent")]
        public async Task<IActionResult> UnassignStudent(ApplicationUser user, Section section, Course course, Department department)
        {
            string code = department.DepartmentCode + course.CourseCode;

            var student = await _userManager.FindByNameAsync(user.UserName);

            Enrollment deleteStudent = _context.Enrollments.Where(e => e.StudentId == student.Id && e.SectionId == section.SectionId).First();
            _context.Enrollments.Remove(deleteStudent);
            _context.SaveChanges();            

            return RedirectToAction("ViewSection", "Admin", new { code, section.SectionNumber });
        }

        [HttpGet, Route("Sections/Edit/{CourseCode}/{SectionNumber}")]
        public IActionResult EditSection(String CourseCode, int SectionNumber)
        {
            return View();
        }
        #endregion section controllers
    }
}
