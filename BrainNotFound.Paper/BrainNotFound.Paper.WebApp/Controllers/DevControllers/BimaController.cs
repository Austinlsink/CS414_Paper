
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using BrainNotFound.Paper.WebApp.Models.BusinessModels;

namespace BrainNotFound.Paper.WebApp.Controllers.DevControllers
{
    public class BimaController : Controller
    {
        
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private PaperDbContext _context;


        public async Task<IActionResult> Run()
        {
            var NewUserFetched = await _userManager.FindByEmailAsync("abmael.silva@me.com");
            await _userManager.AddToRoleAsync(NewUserFetched, "Admin");

            
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
                //Get the user just created
                //var NewUserFetched = await _userManager.FindByEmailAsync(user.Email);


                //Add the user Role to the created user
                //await _userManager.AddToRoleAsync(NewUserFetched, "Admin");

                return RedirectToAction("Instructors", "Admin");
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

        public IActionResult Index()
        {
            /*var d1 = new Department();

            d1.DepartmentCode = "CS";
            d1.DepartmentName = "Computer Science";


            _context.Departments.Add(d1);
            _context.SaveChanges();

            ViewData["Message"] = "Success";
            


            // Created the Roles in the Database
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

            
            await _userManager.AddToRoleAsync(user, "Admin");
            */
            return View();
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
