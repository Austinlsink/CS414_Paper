﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BrainNotFound.Paper.api
{
    [Route("api/CreateTest")]
    public class CreateTestController : Controller
    {
        #region Initialize controllers
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PaperDbContext _context;
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userManager">Sets the UserManager</param>
        /// <param name="context">Sets the database context</param>
        /// 
        public CreateTestController(UserManager<ApplicationUser> userManager, PaperDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        #endregion Initialize Controllers

        [HttpPost, Route("CreateTestSection")]
        public JsonResult CreateTestSection(long TestId, string QuestionTypeName)
        {
            var test = _context.Tests.Find(TestId);
            var questionType = _context.QuestionTypes.Where(qt => qt.Name == QuestionTypeName).First();
            var NewSection = new TestSection()
            {
                QuestionType = questionType,
                IsQuestionSection = true,
                SectionInstructions = DefaultInstruction.TrueFalse
            };

            test.TestSections.Add(NewSection);
            _context.SaveChanges();

            return Json(NewSection);
        }

        // Creates a new Test Schedule
        [HttpPost, Route("NewTestSchedule")]
        public JsonResult NewTestSchedule([FromBody]JObject jsonData)
        {
            // Receiveing the data
            dynamic json = jsonData;
            long testId = json.TestId;
            string startEndDateTime = json.StartEndDateTime;
            bool isTimeUnlimited = bool.Parse((string) json.IsTimeUnlimited);
            int timeLimit = json.TimeLimit;
            JArray studentIds = json.Students;
            JArray sectionIds = json.Sections;


            // Parsing the date
            string startDateTime = startEndDateTime.Substring(0, startEndDateTime.IndexOf(" - "));
            string endDateTime = startEndDateTime.Substring(startEndDateTime.IndexOf(" - ") + 3);
            DateTime parsedStartDateTime = DateTime.ParseExact(startDateTime, "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);
            DateTime parsedEndDateTime   = DateTime.ParseExact(endDateTime,   "MM/dd/yyyy hh:mm tt", new CultureInfo("en-US"), DateTimeStyles.None);

            // Create the new Schedule
            var newTestSchedule = new TestSchedule()
            {
                StartTime = parsedStartDateTime,
                EndTime = parsedEndDateTime,
                TestId = testId,
                TimeLimit = isTimeUnlimited ? 0 : timeLimit,
                IsTimeUnlimited = isTimeUnlimited
            };

            // Assigns all the students in the section to that  
            newTestSchedule.StudentTestAssignments = new List<StudentTestAssignment>();

            // Get all students in sections
            foreach (long sectionId in sectionIds)
            {
                var section = _context.Sections.Include(s => s.Enrollments).Where(s => s.SectionId == sectionId).First();
                foreach (Enrollment en in section.Enrollments)
                {
                    newTestSchedule.StudentTestAssignments.Add(new StudentTestAssignment()
                    {
                        StudentId = en.StudentId
                    });
                }
            }

            _context.TestSchedules.Add(newTestSchedule);
            _context.SaveChanges();




            return Json(new { success = true });
        }

        // Gets all the students in a section
        [HttpGet, Route("GetStudentsInSection/{SectionId}")]
        public JsonResult GetSection(long SectionId)
        {

            var enrollments = _context.Enrollments.Include(e => e.ApplicationUser).Where(e => e.SectionId == SectionId).ToList();
            List<JObject> students = new List<JObject>();

            foreach(Enrollment enrollment in enrollments)
            {
                dynamic student = new JObject();
                student.FirstName = enrollment.ApplicationUser.FirstName;
                student.LastName = enrollment.ApplicationUser.LastName;
                student.Id = enrollment.ApplicationUser.Id;
                students.Add(student);
            }

            return Json(students);
        }       
    }
}