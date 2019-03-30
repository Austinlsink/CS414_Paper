using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper.Models.ViewModels;
using BrainNotFound.Paper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BrainNotFound.Paper.Controllers
{
    public class DataController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }

        #region Roles
        public async Task<IActionResult> AddRoles()
        {
            foreach (var roleName in UserRole.All)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveRoles()
        {
            foreach (string roleName in UserRole.All)
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync(roleName));
                }
            }
            return RedirectToAction("Index", "Data");
        }

        #endregion Roles

        #region Users Admin/Instructors/Students

        public async Task<IActionResult> AddAdministrators()
        {
            var administrators = ApplicationUser.ParseCsv("SampleData/Administrators_Sample_Data.csv");

            foreach (ApplicationUser administrator in administrators)
            {
                // Check if User Name is Taken
                string userName;
                bool userNameTaken = false;
                int userNamePostFix = -1;

                do
                {
                    userNamePostFix += 1;
                    userName = administrator.FirstName.Replace(" ", string.Empty) + administrator.LastName.Replace(" ", string.Empty) + (userNamePostFix == 0 ? "" : userNamePostFix.ToString());
                    var user = await _userManager.FindByNameAsync(userName);
                    userNameTaken = user != null;
                } while (userNameTaken);

                administrator.UserName = userName;

                //Create a new Application User
                var result = await _userManager.CreateAsync(administrator, administrator.Password);


                if (result.Succeeded)
                {
                    //Fetch created user
                    var CreatedUser = await _userManager.FindByEmailAsync(administrator.Email);

                    //Add instructor role to created Application User
                    await _userManager.AddToRoleAsync(CreatedUser, UserRole.Admin);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ViewData["Errors"] += error.Description;
                    }
                }
            }
           
            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveAdministrators()
        {
            var allUsers = _context.ApplicationUsers.ToList();

            foreach (ApplicationUser user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, UserRole.Admin))
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> AddInstructors()
        {
            var instructors = ApplicationUser.ParseCsv("SampleData/Instructors_Sample_Data.csv");

            foreach (ApplicationUser instructor in instructors)
            {
                string userName;
                bool userNameTaken = false;
                int userNamePostFix = -1;

                do
                {
                    userNamePostFix += 1;
                    userName = instructor.FirstName.Replace(" ", string.Empty) + instructor.LastName.Replace(" ", string.Empty) + (userNamePostFix == 0 ? "" : userNamePostFix.ToString());
                    var user = await _userManager.FindByNameAsync(userName);
                    userNameTaken = user != null;
                } while (userNameTaken);

                instructor.UserName = userName;

                //Create a new Application User
                var result = await _userManager.CreateAsync(instructor, instructor.Password);


                if (result.Succeeded)
                {
                    //Fetch created user
                    var CreatedUser = await _userManager.FindByEmailAsync(instructor.Email);

                    //Add instructor role to created Application User
                    await _userManager.AddToRoleAsync(CreatedUser, UserRole.Instructor);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ViewData["Errors"] += error.Description;
                    }
                }
            }

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveInstructors()
        {
            var allUsers = _context.ApplicationUsers.ToList();

            foreach (ApplicationUser user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, UserRole.Instructor))
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> AddStudents()
        {
            var students = ApplicationUser.ParseCsv("SampleData/Students_Sample_Data.csv");

            foreach (ApplicationUser student in students)
            {

                // Check if User Name is available
                string userName;
                bool userNameTaken = false;
                int userNamePostFix = -1;

                do
                {
                    userNamePostFix += 1;
                    userName = student.FirstName.Replace(" ", string.Empty) + student.LastName.Replace(" ", string.Empty) + (userNamePostFix == 0 ? "" : userNamePostFix.ToString());
                    var user = await _userManager.FindByNameAsync(userName);
                    userNameTaken = user != null;
                } while (userNameTaken);

                student.UserName = userName;

                string[] dob = student.DateOfBirth.Split('/');
                int year = Int32.Parse(dob[2]);
                int day = Int32.Parse(dob[1]);
                int month = Int32.Parse(dob[0]);

                var date = new DateTime(year, month, day);

                student.DOB = date;

                student.Password = student.FirstName + student.LastName;

                //Create a new Application User
                var result = await _userManager.CreateAsync(student, student.Password);

                if (result.Succeeded)
                {
                    //Fetch created user
                    var CreatedUser = await _userManager.FindByEmailAsync(student.Email);

                    //Add instructor role to created Application User
                    await _userManager.AddToRoleAsync(CreatedUser, UserRole.Student);

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ViewData["Errors"] += error.Description;
                    }
                }
            }

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveStudents()
        {
            var allUsers = _context.ApplicationUsers.ToList();

            foreach (ApplicationUser user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, UserRole.Student))
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            return RedirectToAction("Index", "Data");
        }

        #endregion Users Admin/Instructors/Students

        #region Regular Tables
        public async Task<IActionResult> AddDepartments()
        {
            var departments = Department.ParseCsv("SampleData/Department_Sample_Data.csv");

            foreach (Department department in departments)
            {
                await _context.Departments.AddAsync(department);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveDepartments()
        {
            _context.Departments.RemoveRange(_context.Departments);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> AddCourses()
        {
            var courses = Course.ParseCsv("SampleData/Course_Sample_Data.csv");

            foreach (Course c in courses)
            {
                var d = _context.Departments.Where(dd => dd.DepartmentCode == c.DepartmentCode).First();
                c.Department = d;
                _context.Courses.Add(c);
                await _context.Courses.AddAsync(c);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveCourses()
        {
            _context.Courses.RemoveRange(_context.Courses);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> AddSections()
        {
            var sections = SampleSection.ParseCsv("SampleData/Sections_Sample_Data.csv");

            foreach (var sampleSection in sections)
            {
                var section = new Section();
                var sectionMeetingTimes = new List<SectionMeetingTime>();

                // Finds the course that the section belongs too
                var courseName = sampleSection.CourseName.Substring(7);
                var course = _context.Courses.Where(c => c.Name == courseName).First();
                section.Course = course;

                // Assembles the days that the course meets

                section.TimesMet = SectionMeetingTime.Parse(sampleSection.DaysMet);

                // Finds the Instructor that teaches this section
                var instructor = _context.ApplicationUsers.Where(i => i.FirstName == sampleSection.FirstName && i.LastName == sampleSection.LastName).First();
                section.ApplicationUser = instructor;

                // Sets the Location and capacity of the Section
                section.Location = sampleSection.Location;
                section.Capacity = sampleSection.Capacity;

                // Sets the Section Number
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

                section.SectionNumber = sectionNumber;
                section.Course = course;

                _context.Sections.Add(section);
                await _context.SaveChangesAsync();
            }
            

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveSections()
        {
            _context.Sections.RemoveRange(_context.Sections);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> AddEnrollments()
        {
            // Get required data lists
            var allCourses = _context.Courses.ToList();
            var allSections = _context.Sections.ToList();
            var allSectionMeetingTimes = _context.SectionMeetingTimes.ToList();
            var allStudents = (await _userManager.GetUsersInRoleAsync("Student")).ToList();

            List<Enrollment> enrollments = new List<Enrollment>();

            // Populate Enrollment table
            foreach (var student in allStudents)
            {
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

                    // Select a random section in the selectedCourse and make sure it doesn't conflict with classes
                    // the student is already enrolled in
                    bool isBeingTaken = false; // Used to check if the student is already taking a section in that course
                    bool doesNotConflict = false; // Used in check in main do while
                    Section selectedSection = new Section();
                    do
                    {
                        // Choose a course with sections in it
                        do
                        {
                            selectedCourse = allCourses.ElementAt(rand.Next(0, allCourses.Count));
                        } while (allSections.Where(S => S.CourseId == selectedCourse.CourseId).Count() <= 0); 

                        // Make sure the student isn't already taking a section in this course
                        foreach (var studentEnrollment in enrollments.Where(E => E.StudentId == student.Id))
                        {
                            foreach (var enrolledSection in allSections.Where(S => S.SectionId == studentEnrollment.SectionId))
                            {
                                if (enrolledSection.CourseId == selectedCourse.CourseId)
                                {
                                    isBeingTaken = true;
                                }
                            }
                        }

                        // If the student is not already enrolled in a section in that course
                        if (isBeingTaken == false)
                        {
                            selectedSection = allSections.Where(S => S.CourseId == selectedCourse.CourseId).ElementAt(rand.Next(0, allSections.Where(S => S.CourseId == selectedCourse.CourseId).Count()));

                            if (enrollments.Where(E => E.StudentId == student.Id && E.SectionId == selectedSection.SectionId).Count() <= 0)
                            {
                                doesNotConflict = true; // Reset to "true" in case a new section needed to be selected
                                List<SectionMeetingTime> concurrentSectionMeetingTimes = new List<SectionMeetingTime>();
                                foreach (var enrollment in enrollments.Where(E => E.StudentId == student.Id))
                                {
                                    // The times that the student already has classes at
                                    foreach (var enrolledSectionMeetingTime in allSectionMeetingTimes.Where(SMT => SMT.SectionId == enrollment.SectionId).ToList())
                                    {
                                        foreach (var selectedSectionMeetingTime in allSectionMeetingTimes.Where(SMT => SMT.SectionMeetingTimeId == selectedSection.SectionId).ToList())
                                        {
                                            if (enrolledSectionMeetingTime.Day == selectedSectionMeetingTime.Day // If it meets on the same day...
                                                && ((selectedSectionMeetingTime.StartTime >= enrolledSectionMeetingTime.StartTime // If it starts after or at...
                                                       && selectedSectionMeetingTime.EndTime <= enrolledSectionMeetingTime.EndTime)  // ... and ends before or at (OR)
                                                   || (selectedSectionMeetingTime.StartTime <= enrolledSectionMeetingTime.StartTime // If it starts before or at...
                                                       && selectedSectionMeetingTime.EndTime >= enrolledSectionMeetingTime.EndTime))) // ... and ends after or at...
                                            {
                                                doesNotConflict = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    } while (doesNotConflict == false);

                    Enrollment newEnrollment = new Enrollment
                    {
                        StudentId = student.Id,
                        SectionId = selectedSection.SectionId
                    };
                    enrollments.Add(newEnrollment);
                    _context.Enrollments.Add(newEnrollment);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Data");
        }

        public async Task<IActionResult> RemoveEnrollments()
        {
            _context.Enrollments.RemoveRange(_context.Enrollments);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Data");
        }

        #endregion Regular Tables

        #region Services Registration
        //Dependencies for Managing users
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;

        public DataController(
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

        #endregion Services Registration
    }
}