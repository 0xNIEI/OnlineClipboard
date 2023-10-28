using Microsoft.AspNetCore.Mvc;
using OnlineClipboard.Models;
using System.Diagnostics;

namespace OnlineClipboard.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(ILogger<ApplicationController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeTheme(string theme)
        {
            if (theme == "light")
            {
                if (HttpContext.Request.Cookies.Any(x => x.Key == "__Secure-DarkMode" && x.Value == "false"))
                {
                    return Ok();
                }
                HttpContext.Response.Cookies.Delete("__Secure-DarkMode");
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(12),
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true,
                    Path = "/",
                    HttpOnly = true
                };
                HttpContext.Response.Cookies.Append("__Secure-DarkMode", "false", options);
            }
            else
            {
                if (HttpContext.Request.Cookies.Any(x => x.Key == "__Secure-DarkMode" && x.Value == "true"))
                {
                    return Ok();
                }
                else
                {
                    var options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMonths(12),
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        IsEssential = true,
                        Path = "/",
                        HttpOnly = true
                    };
                    HttpContext.Response.Cookies.Append("__Secure-DarkMode", "true", options);
                }

            }
            return Ok();
        }
    }
}