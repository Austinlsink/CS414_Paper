
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

        private PaperDbContext _context;




        //public IActionResult Run()
        public async Task<IActionResult> Run()
        {
            
            var user = await _userManager.GetUserAsync(User);
            ViewData["Message"] = user.FullName;

            

            return View("TestView");
        }

        public async Task<IActionResult> AddInstructorsToDatabase()
        {

            using (var reader = new StreamReader("SampleData/Instructor_Sample_Data.csv"))
            using (var csv = new CsvReader(reader))
            {

                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;


                var instructors = csv.GetRecords<ApplicationUser>();
                foreach (ApplicationUser model in instructors)
                {

                    var newInstructor = new ApplicationUser()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Salutation = model.Salutation,
                        UserName = model.FirstName + model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,

                    };

                    //Create a new Application User
                    var result = await _userManager.CreateAsync(newInstructor, model.Password);


                    if (result.Succeeded)
                    {
                        //Fetch created user
                        var CreatedUser = await _userManager.FindByEmailAsync(model.Email);

                        //Add instructor role to created Application User
                        await _userManager.AddToRoleAsync(CreatedUser, "Instructor");

                        await _userManager.AddClaimAsync(CreatedUser, new Claim("FullName", CreatedUser.FirstName + " " + CreatedUser.LastName));
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

            return View("TestView");
        }

        public async Task<IActionResult> InitialSetup()
        {
            //Create Roles
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
                await _userManager.AddClaimAsync(NewUserFetched, new Claim("FullName", NewUserFetched.FirstName + " " + NewUserFetched.LastName));
                var signinResult = await _signInManager.PasswordSignInAsync("AbmaelSilva", "PaperBrain2019!", false, false);

                if (signinResult.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ViewData["Message"] += error.Description;
                }
            }

            return View("TestView");

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
