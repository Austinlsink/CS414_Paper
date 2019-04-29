using Microsoft.AspNetCore.Mvc;

namespace BrainNotFound.Paper.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/error")]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode.HasValue)
            {
                // here is the trick
                this.HttpContext.Response.StatusCode = statusCode.Value;
            }

            switch(statusCode.Value)
            {
                case 404:
                    return View("Error404");
                case 500:
                    return View("Error500");
                default:
                    return View("Error500");
            }
            
        }
    }
}