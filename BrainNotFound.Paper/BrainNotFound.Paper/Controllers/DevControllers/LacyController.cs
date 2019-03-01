
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
    public class LacyController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;




        //public IActionResult Run()
        public async Task<IActionResult> Run()
        {
            return RedirectToAction("AddStudentsToDb", "Lacy");
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

            return RedirectToAction("AddAdministratorsToDb", "Lacy");
        }

        // Populates the database with the Admistrators Data
        public async Task<IActionResult> AddAdministratorsToDb()
        {
            //Create a Identity User
            ApplicationUser user = new ApplicationUser()
            {
                Email = "abmael.silva@me.com",
                UserName = "LacyTesdall",
                PhoneNumber = "8502883660",
                FirstName = "Lacy",
                LastName = "Tesdall",
                Salutation = "Mrs"
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
        public async Task<IActionResult> AddStudentsToDb()
        {
            Random num = new Random();
            int randomPassword;
            
           

            using (var reader = new StreamReader("SampleData/students.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                var students = csv.GetRecords<ApplicationUser>();
                foreach (ApplicationUser student in students)
                {
                    string[] dob = student.DateOfBirth.Split('/');
                    int year = Int32.Parse(dob[2]);
                    int day = Int32.Parse(dob[1]);
                    int month = Int32.Parse(dob[0]);

                    var date = new DateTime(year, month, day);
                    
                    student.DOB = date;

                    randomPassword = num.Next(100, 1000);
                    student.UserName = student.FirstName + student.LastName;
                    student.Password = student.FirstName + student.LastName + randomPassword.ToString();

                    

                    //Create a new Application User
                    var result = await _userManager.CreateAsync(student, student.Password);


                    if (result.Succeeded)
                    {
                        //Fetch created user
                        var CreatedUser = await _userManager.FindByEmailAsync(student.Email);

                        //Add instructor role to created Application User
                        await _userManager.AddToRoleAsync(CreatedUser, "Student");

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

        // Constructor
        public LacyController(
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
