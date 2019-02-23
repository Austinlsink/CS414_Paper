﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BrainNotFound.Paper.WebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace BrainNotFound.Paper.WebApp.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("Student")]
    public class StudentController : Controller
    {
        [HttpGet, Route("")]
        [HttpGet, Route("Index")]
        [HttpGet, Route("Dashboard")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("Courses")]
        public IActionResult Courses()
        {
            return View();
        }

        [HttpGet, Route("Courses/{CourseCode}")]
        public IActionResult ViewCourse(String CourseCode)
        {
            return View();
        }

        [HttpGet, Route("Grades")]
        public IActionResult Grades()
        {
            return View();
        }

        [HttpGet, Route("Profile")]
        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet, Route("Profile/Edit")]
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpGet, Route("Tests")]
        public IActionResult Tests()
        {
            return View();
        }

        [HttpGet, Route("Tests/TestSummary")]
        public IActionResult TestSummary()
        {
            return View();
        }

        [HttpGet, Route("Settings")]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet, Route("Tests/TakeTest")]
        public IActionResult TakeTest()
        {
            return View();
        }

        [HttpGet, Route("Tests/ReviewTest")]
        public IActionResult ReviewTest()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
