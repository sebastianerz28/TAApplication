using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using TAApplication.Areas.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        UserManager<TAUser> _um;

        public HomeController(ILogger<HomeController> logger, UserManager<TAUser> um)
        {
            _logger = logger;
            _um = um;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Applicant")]
        public IActionResult ApplicationCreate()
        {
            return View();
        }

        [Authorize(Roles = "Professor, Admin, Applicant")]
        public IActionResult ApplicantList()
        {
            return View();
        }

        [Authorize(Roles = "Professor, Admin, Applicant")]
        public IActionResult ApplicationDetails()
        {
            if (_um.GetUserAsync(User).Result.Unid != "u0000000" && _um.GetRolesAsync(_um.GetUserAsync(User).Result).Result.FirstOrDefault().Equals("Applicant"))
            {
                return View("NotAuthorized");
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}