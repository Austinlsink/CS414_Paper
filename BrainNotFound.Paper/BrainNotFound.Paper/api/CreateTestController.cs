using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BrainNotFound.Paper.api
{
    [Route("api")]
    public class CreateTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("GetSection")]
        public string GetSection()
        {
            return "BimaIsCool";
        }       
    }
}