using Microsoft.AspNetCore.Mvc;
using OnlineClipboard.Models;
using System.Diagnostics;

namespace OnlineClipboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
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
                if (HttpContext.Request.Cookies.Any(x => x.Key == "Darkmode" && x.Value == "false"))
                {
                    return Ok();
                }
                HttpContext.Response.Cookies.Delete("DarkMode");
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddMonths(12);
                options.IsEssential = true;
                options.Path = "/";
                HttpContext.Response.Cookies.Append("DarkMode", "false", options);
            }
            else
            {
                if (HttpContext.Request.Cookies.Any(x => x.Key == "Darkmode" && x.Value == "true"))
                {
                    return Ok();
                }
                else
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTime.Now.AddMonths(12);
                    options.IsEssential = true;
                    options.Path = "/";
                    HttpContext.Response.Cookies.Append("DarkMode", "true", options);
                }

            }
            return Ok();
        }
    }
}