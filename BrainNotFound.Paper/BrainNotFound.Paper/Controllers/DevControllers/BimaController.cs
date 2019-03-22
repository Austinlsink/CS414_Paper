
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
using BrainNotFound.Paper.Models.ViewModels;

namespace BrainNotFound.Paper.Controllers.DevControllers
{
    public class BimaController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;

        //public IActionResult Run()
        public string Run()
        {
            string ReturnMessage = string.Empty;
            Department dept = new Department()
            {
                DepartmentCode = "BI"
            };

            if (TryValidateModel(dept))
                ReturnMessage += "True";
            else
                ReturnMessage += ModelState["DepartmentName"].Errors.First().ErrorMessage;


            return ReturnMessage;
        }

        // Populates the database with the sample Departments info
        public async Task<IActionResult> AddDepartmentsToDb()
        {
            var departments = Department.ParseCsv("SampleData/Department_Sample_Data.csv");
            var existingDepartments = _context.Departments;
            foreach (Department department in departments)
            {
                
                if ((existingDepartments.Where(d => d.DepartmentCode == department.DepartmentCode)).Any())
                {
                    var existingDepartment = existingDepartments.Where(d => d.DepartmentCode == department.DepartmentCode).First();

                    // Check if all information inside the object is the same as in the Database
                    // [TODO: Create as Equals() f(x)]
                    //if (existingDepartment.DepartmentName == department.DepartmentName) // On ID instead?
                    if (existingDepartment.Equals(department))
                    {
                        // Will Notify user that department already exists
                    }
                    else
                    {
                        // Update Record
                        // Kara: Now has a function to do so.
                        existingDepartment.Update(department);
                    }
                }
                else
                {
                    await _context.Departments.AddAsync(department);
                }
                // if department is in _context.Departments
                
            }
            _context.SaveChanges();

            return RedirectToAction("AddStudentsToDb", "Bima");
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
