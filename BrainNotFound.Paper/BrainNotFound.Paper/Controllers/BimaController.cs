using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BrainNotFound.Paper.Controllers
{
    public class BimaController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = DateTime.Now.ToString("ddd, MMM d, hh:mm tt");
            return View("TestView");
        }
    }
}