using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrainNotFound.Paper.Models.BusinessModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainNotFound.Paper.api
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {

        [HttpPost, Route("New")]
        public IActionResult New([FromForm] Department department)
        {
            return Json("Bima Did good");
        }

    }
}