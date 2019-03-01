
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
    public class BimaController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;




        //public IActionResult Run()
        public async Task<IActionResult> Run()
        {
            using (var reader = new StreamReader("SampleData/Course_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;

                var courses = csv.GetRecords<Course>();
                int counter = 0;
                foreach (Course course in courses)
                {
                    var department = _context.Departments.Where(d => d.DepartmentCode == course.DepartmentCode).First();
                    if(department == null)
                    {
                        ViewData["Message"] = "There was a Null Value";
                    }


                    _context.Courses.Add(course);
                    course.Department = department;
                    counter++;
                }

               //_context.SaveChanges();
            }

            return View("TestView");
        }

        [HttpGet, Route("Initialize")]
        public async Task<IActionResult> Initialize()
        {
            

            return RedirectToAction("AddRolesToDb", "Bima");
        }
    
        // Populates the database with the User Roles
        public async Task<IActionResult> AddRolesToDb()
        {
            string[] roleNames = { "Admin", "Instructor", "Student" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            return RedirectToAction("AddAdministratorsToDb", "Bima");
        }

        // Populates the database with the Admistrators Data
        public async Task<IActionResult> AddAdministratorsToDb()
        {
            //Create a Identity User
            ApplicationUser user = new ApplicationUser()
            {
                Email = "abmael.silva@me.com",
                UserName = "AbmaelSilva",
                PhoneNumber = "8502883660",
                FirstName = "Abmael",
                LastName = "Fernandes da Silva",
                Salutation = "Mr"
            };

            //Add the User to the IdentityDbContext
            var result = await _userManager.CreateAsync(user, "PaperBrain2019!");

            //Check if user was created successfully
            if (result.Succeeded)
            {
                //Add the user Role to the created user
                var NewUserFetched = await _userManager.FindByEmailAsync(user.Email);
                await _userManager.AddToRoleAsync(NewUserFetched, "Admin");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ViewData["Message"] += error.Description;
                }
            }

            return RedirectToAction("AddInstructorsToDb", "Bima");
        }

        // Populates the Database with the Sample Instructors account
        public async Task<IActionResult> AddInstructorsToDb()
        {
            using (var reader = new StreamReader("SampleData/Instructor_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                var instructors = csv.GetRecords<ApplicationUser>();
                foreach (ApplicationUser instructor in instructors)
                {

                    instructor.UserName = instructor.FirstName + instructor.LastName;

                    //Create a new Application User
                    var result = await _userManager.CreateAsync(instructor, instructor.Password);


                    if (result.Succeeded)
                    {
                        //Fetch created user
                        var CreatedUser = await _userManager.FindByEmailAsync(instructor.Email);

                        //Add instructor role to created Application User
                        await _userManager.AddToRoleAsync(CreatedUser, "Instructor");

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ViewData["Message"] += error.Description;
                        }
                    }
                }
            }

            return RedirectToAction("AddDepartmentsToDb", "Bima");
        }

        // Populates the database with the sample Departments info
        public async Task<IActionResult> AddDepartmentsToDb()
        {
            using (var reader = new StreamReader("SampleData/Department_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                var departments = csv.GetRecords<Department>();
                foreach (Department model in departments)
                {
                    await _context.Departments.AddAsync(model);
                }
                _context.SaveChanges();
            }

            return RedirectToAction("ForceLogin", "Account");
        }

        // Constructor
        public BimaController(
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
