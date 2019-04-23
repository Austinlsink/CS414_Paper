using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace BrainNotFound.Paper.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager; // Access to all user information
        private readonly PaperDbContext _context;                   // Access to the database

        #region admin controllers
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager">Gives access to all of the user profiles</param>
        /// <param name="context">Gives access to the databse</param>
        public AdminController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Displays the Index page for the Admin profile. Also called "Dashboard"
        /// </summary>
        /// <returns>Index View</returns>
        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public async Task<IActionResult> Index()
        {
            // Fetch Number of Students in the system
            SqlParameter[] @params1 = {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };
            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetNumberOfStudents", @params1);
            ViewBag.NumberOfStudents = @params1[0].Value;

            // Fetch the Number of Students that have not been assigned to a course
            var allStudents = await _userManager.GetUsersInRoleAsync("Student");
            var enrollments = _context.Enrollments.ToList();

            List<ApplicationUser> studentsNotEnrolled = new List<ApplicationUser>();
            foreach (ApplicationUser student in allStudents)
            {
                if (!(enrollments.Where(x => x.StudentId == student.Id).Any()))
                {
                    studentsNotEnrolled.Add(student);
                }
            }
            ViewBag.StudentsNotEnrolled = studentsNotEnrolled;


            //Fetch the number of Instructors in the system
            SqlParameter[] @params2 = {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };
            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetNumberOfInstructors", @params2);
            ViewBag.NumberOfInstructors = @params2[0].Value;

            // Fetch the Number of Students that have not been assigned to teach
            var allInstructors = await _userManager.GetUsersInRoleAsync("Instructor");
            var sections = _context.Sections.ToList();

            List<ApplicationUser> instructorsNotAssigned = new List<ApplicationUser>();
            foreach (ApplicationUser instructor in allInstructors)
            {
                if (!(sections.Where(x => x.InstructorId == instructor.Id).Any()))
                {
                    instructorsNotAssigned.Add(instructor);
                }
            }
            ViewBag.InstructorsNotAssigned = instructorsNotAssigned;

            //Fetch the number of Departments in the system
            SqlParameter[] @params3 = {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };
            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetNumberOfDepartments", @params3);
            ViewBag.NumberOfDepartments = @params3[0].Value;

            ViewBag.Departments = _context.Departments.ToList();

            // Fetch the number of Courses in the system
            ViewBag.Courses = _context.Courses.ToList();

            //Fetch the number of Tests in the system
            SqlParameter[] @params4 = {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
            };
            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetNumberOfTests", @params4);
            ViewBag.NumberOfTests = @params4[0].Value;

            return View();
        }

        /// <summary>
        /// Displays the Settings page for the Admin profile.
        /// </summary>
        /// <returns>Settings View</returns>
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
        /// <returns>Administrators View</returns>
        [HttpGet, Route("Administrators")]
        public async Task<IActionResult> Administrators()
        {
            var allAdministrators = (await _userManager.GetUsersInRoleAsync("Admin")).ToList();
            ViewBag.adminList = allAdministrators;
            return View();
        }

        /// <summary>
        /// Views a specific administrator's information
        /// </summary>
        /// <param name="username">The specific admin to look up</param>
        /// <returns>Administrator's partial view</returns>
        [HttpGet, Route("Administrators/{UserName}")]
        public async Task<IActionResult> ViewAdministrator(String username)
        {
            var admin = await _userManager.FindByNameAsync(username);
            ViewBag.admin = admin;

            return PartialView();
        }

        /// <summary>
        /// Allows the Administrator to edit another administrator's profile
        /// </summary>
        /// <param name="UserName">Search criteria for finding the correct administrator</param>
        /// <returns>Partial View for editing an administator</returns>
        [HttpGet, Route("Administrators/Edit/{UserName}")]
        public async Task<IActionResult> EditAdministrator(String UserName)
        {
            ApplicationUser admin = await _userManager.FindByNameAsync(UserName);
            ViewBag.admin = admin;
            return PartialView();
        }

        /// <summary>
        /// Saves edited changes to an administrator's profile
        /// </summary>
        /// <param name="user">ApplicationUser object that contains the new information</param>
        /// <returns>Redirects to the Administrators page</returns>
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

        #endregion create administrator controllers

        #region create instructor controllers
        /// <summary>
        /// Allows the admin to view all of the instructors
        /// </summary>
        /// <returns>Instructors View</returns>
        [HttpGet, Route("Instructors")]
        public async Task<IActionResult> Instructors()
        {
            var allInstructors = (await _userManager.GetUsersInRoleAsync("Instructor")).ToList();
            ViewBag.allInstructors = allInstructors;
            return View();
        }

        /// <summary>
        /// Allows the admin to view a specific instructor and his information
        /// </summary>
        /// <param name="username">Search criteria for finding the correct instructor</param>
        /// <returns>Specific instructor and his associated information</returns>
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

        /// <summary>
        /// Allows the admin to edit a specific instructor's profile information
        /// </summary>
        /// <param name="UserName">Search criteria for finding the specific instructor</param>
        /// <returns>partial View to edit instructor</returns>
        [HttpGet, Route("Instructors/Edit/{UserName}")]
        public async Task<IActionResult> EditInstructor(String UserName)
        {
            ApplicationUser instructor = await _userManager.FindByNameAsync(UserName);
            ViewBag.instructor = instructor;
            return PartialView();
        }

        /// <summary>
        /// Allows the admin to save the edited changes on an instructor profile
        /// </summary>
        /// <param name="user">ApplicationUser object that contains the new information</param>
        /// <returns>Redirects to the Instructors page</returns>
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

            return RedirectToAction("Instructors", "Admin");
        }
        #endregion instructor controllers

        #region admin profile controllers
        /// <summary>
        /// Allows the admin to view his profile and personal information
        /// </summary>
        /// <returns>Profile View</returns>
        [HttpGet, Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.profile = admin;
            return View();
        }

        /// <summary>
        /// Allows the admin to edit his profile
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Profile/Edit")]
        public async Task<IActionResult> EditProfile()
        {
            ApplicationUser admin = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.admin = admin;
            return View();
        }

        /// <summary>
        /// Allows the admin to save the edited changes to his profile
        /// </summary>
        /// <param name="user">ApplicationUser object that contains the new information</param>
        /// <returns>Redirects to the Profile Page</returns>
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

        /// <summary>
        /// Displays all of the students
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Students")]
        public async Task<IActionResult> Students()
        {
            var allStudents = (await _userManager.GetUsersInRoleAsync("Student")).OrderBy(o => o.FirstName).ToList();
            ViewBag.allStudents = allStudents;
            return View();
        }

        /// <summary>
        /// Allows the Admin to view a specific student, the student's personal information, and all of the classes that the student is enrolled in
        /// </summary>
        /// <param name="username">Search criteria for finding the specific student</param>
        /// <returns>Specific Student page</returns>
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

        /// <summary>
        /// Allows the admin to edit a specific student's profile information
        /// </summary>
        /// <param name="UserName">Search criteria for finding the specific student</param>
        /// <returns></returns>
        [HttpGet, Route("Students/Edit/{UserName}")]
        public async Task<IActionResult> EditStudent(String UserName)
        {
            ApplicationUser student = await _userManager.FindByNameAsync(UserName);
            ViewBag.student = student;
            return PartialView();
        }

        /// <summary>
        /// Allows the admin to save the edited changes to the student's profile
        /// </summary>
        /// <param name="user">ApplicationUser object that contains the new information</param>
        /// <returns>Redirects to the Students page</returns>
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

            return RedirectToAction("Students", "Admin");
        }

        #endregion student controllers 

        #region Department controllers

        /// <summary>
        /// Displays the list of departments
        /// </summary>
        /// <returns>Departments View</returns>
        [HttpGet, Route("Departments")]
        public ActionResult Departments()
        {
            var courses = _context.Courses.ToList();
            List<Department> departments = _context.Departments.ToList();

            ViewBag.courses = courses;
            ViewBag.departmentList = departments;
            return View();
        }

        /// <summary>
        /// Allows the admin to edit a department's information
        /// </summary>
        /// <param name="Id">Search criteria for finding the specific department</param>
        /// <returns>Partial View for Edit Department</returns>
        [HttpGet, Route("Department/Edit/{Id}")]
        public IActionResult EditDepartment(long Id)
        {
            var department = _context.Departments.Find(Id);
            ViewBag.department = department;
            return PartialView();
        }

        /// <summary>
        /// Allows the admin to save the edited changes for the department
        /// </summary>
        /// <param name="dept">Department object that contains the new information</param>
        /// <returns>Redirects to the Departments page</returns>
        [HttpPost, Route("Department/Edit/{Id}")]
        public IActionResult EditDepartment(Department dept)
        {
            var depart = _context.Departments.Find(dept.DepartmentId);
            depart.DepartmentCode = dept.DepartmentCode;
            depart.DepartmentName = dept.DepartmentName;

            _context.SaveChanges();

            return RedirectToAction("Departments", "Admin");
        }

        /// <summary>
        /// Allows the admin to create a new department
        /// </summary>
        /// <param name="model">Department object that contains the new department to save</param>
        /// <returns>Redirects to the Departments page</returns>
        [HttpPost, Route("Departments/New")]
        public IActionResult NewDepartment(Department model)
        {
            if (!ModelState.IsValid)
                return PartialView();

            _context.Departments.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Departments", "Admin");
        }

        #endregion Department controllers

        #region Course controllers

        /// <summary>
        /// Allows the admin to view all of the courses
        /// </summary>
        /// <returns>Courses View</returns>
        [HttpGet, Route("Courses")]
        public IActionResult Courses()
        {
            var courses = _context.Courses.OrderBy(o => o.CourseCode).ToList();
            var departments = _context.Departments.OrderBy(o => o.DepartmentName).ToList();

            ViewBag.departmentList = departments;
            ViewBag.courseList = courses;
            return View();
        }

        /// <summary>
        /// Allows the admin to view a specific course and all of its information
        /// </summary>
        /// <param name="code">The DepartmentCode and CourseCode</param>
        /// <returns>Specific course view</returns>
        [HttpGet, Route("Courses/{code}")] // ex: SP101
        public async Task<IActionResult> ViewCourse(string code)
        {
            // Parsing code into the DepartmentCode and the CourseCode
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

            // Find the department and add it to the ViewBag
            var department = _context.Departments.Where(d => d.DepartmentCode == departmentCode).First();
            ViewBag.department = department;

            // Grab all of the departments for editing purposes
            var departments = _context.Departments.ToList();
            ViewBag.departmentList = departments;

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
        #endregion course controllers

        #region Section controllers

        /// <summary>
        /// Allows the admin to create a new section
        /// </summary>
        /// <param name="code">The DepartmentCode and the CourseCode</param>
        /// <returns>New Section View</returns>
        [HttpGet, Route("Sections/New/{code}")]
        public async Task<IActionResult> NewSection(string code)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

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

        /// <summary>
        /// Allows the admin to save a new section and its information
        /// </summary>
        /// <param name="code">DepartmentCode and CourseCode</param>
        /// <param name="section">Section object that contains the needed information</param>
        /// <param name="daysMet">Array of all the days this section meets</param>
        /// <param name="startTime">Start time for when the section meets</param>
        /// <param name="endTime">End time for when the section dismisses</param>
        /// <returns>Redirects to the Index</returns>
        [HttpPost, Route("Sections/New/{code}")]
        public async Task<IActionResult> NewSection(String code, Section section, string[] daysMet, DateTime startTime, DateTime endTime)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

            if (section.InstructorId == null || startTime == null || endTime == null)
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
        /// Allows the admin to view a specific section
        /// </summary>
        /// <param name="code">DepartmentCode + CourseCode</param>
        /// <param name="sectionNumber">section number for the corresonding course</param>
        /// <returns>View Section View</returns>
        [HttpGet, Route("Sections/View/{code}/{sectionNumber}")]
        public async Task<IActionResult> ViewSection(string code, int sectionNumber)
        {
            string departmentCode = code.Substring(0, 2);
            string courseCode = code.Substring(2, 3);

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
        /// Allows the admin to reassign an instructor to a section
        /// </summary>
        /// <param name="user">Requested instructor to assign to the section</param>
        /// <param name="section">The section being reassigned a new instructor</param>
        /// <param name="course">Course that the section belongs to</param>
        /// <param name="department">Department that the section belongs to</param>
        /// <returns></returns>
        [HttpPost, Route("ReassignInstructor")]
        public async Task<IActionResult> ReassignInstructor(ApplicationUser user, Section section, Course course, Department department)
        {
            string code = department.DepartmentCode + course.CourseCode;

            // Find the instructor to reassign to the specified section
            var instructor = await _userManager.FindByNameAsync(user.UserName);

            // Find the specified section to reassign the instructor
            var sect = _context.Sections.Where(s => s.SectionId == section.SectionId).First();
            sect.InstructorId = instructor.Id;

            _context.SaveChanges();

            return RedirectToAction("ViewSection", "Admin", new { code, section.SectionNumber });
        }

        /// <summary>
        /// Allows the admin to assign a student to a section
        /// </summary>
        /// <param name="user">ApplicationUser object that contains the student information</param>
        /// <param name="section">Specific section</param>
        /// <param name="course">Course object that contains information for the section</param>
        /// <param name="department">Department object that contains information for the section</param>
        /// <returns>Redirects to the ViewSection page</returns>
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

        /// <summary>
        /// Allows te admin to unassign a student from a section
        /// </summary>
        /// <param name="user">Student to be removed</param>
        /// <param name="section">Section that the student is being removed from</param>
        /// <param name="course">Course object that contains necessary information for the section</param>
        /// <param name="department">Department object that contains necessary information for the section</param>
        /// <returns>Redirects to the ViewSection View</returns>
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

        /// <summary>
        /// Allows the admin to edit a section
        /// </summary>
        /// <param name="CourseCode">DepartmentCode and CourseCode</param>
        /// <param name="SectionNumber">Section number that is being edited</param>
        /// <returns></returns>
        [HttpGet, Route("Sections/Edit/{CourseCode}/{SectionNumber}")]
        public IActionResult EditSection(String CourseCode, int SectionNumber)
        {
            return View();
        }
        #endregion section controllers
    }
}
