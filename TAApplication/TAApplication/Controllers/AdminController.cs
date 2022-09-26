using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TAApplication.Areas.Data;
using TAApplication.Models;


namespace TAApplication.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        UserManager<TAUser> _um;
        RoleManager<IdentityRole> _rm;
        public AdminController(ILogger<AdminController> logger, UserManager<TAUser> um, RoleManager<IdentityRole> rm)
        {
            _logger = logger;
            _um = um;
            _rm = rm;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Change_Role(string user_id, string role, string add_remove)
        {
            return Ok();
        }



    }
}
