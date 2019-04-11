
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
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BrainNotFound.Paper.Controllers.DevControllers
{
    public class NickController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PaperDbContext _context;


        //public IActionResult Run()
        public async Task<IActionResult> Run()
        {

            return View("TestView");
        }

        public string NickSandbox()
        {
            string StudentId = "48ed6229-5496-4e72-9d9f-08ccac22f14f";

            SqlParameter[] @params =
                                    {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
                            };


            _context.Database.ExecuteSqlCommand("exec @returnVal=dbo.GetNumberOfTests", @params);
            
            return (@params[0].Value).ToString(); //result is 29 

            
        }
        // Constructor
        public NickController(
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
