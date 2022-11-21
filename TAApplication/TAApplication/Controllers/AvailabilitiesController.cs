using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    [Authorize]
    public class AvailabilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TAUser> _userManager;
        public AvailabilitiesController(ApplicationDbContext context, UserManager<TAUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Availabilities

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Applicant")]
        public async Task SetSchedule(string slots, string userId)
        {
            Availability a = JsonSerializer.Deserialize<Availability>(slots);
            a.TAUser = _userManager.FindByIdAsync(userId).Result;
            _context.Availabilities.Update(a);
            await _context.SaveChangesAsync();
        }
        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public string GetSchedule(string userid)
        {
            if (AvailabilityExists(userid))
            {
                var result = _context.Availabilities.Include("TAUser").First(c => c.TAUser.Id == userid);
                var days = new { Monday = result.Monday, Tuesday = result.Tuesday, Wednesday = result.Wednesday, Thursday = result.Thursday, Friday = result.Friday };
                return days.ToJson();
            }
            else
            {
                return null;
            }
        }


        private bool AvailabilityExists(string userid)
        {
            var temp = _context.Availabilities.Include("TAUser").Any(e => e.TAUser.Id == userid);
            return temp;
        }
    }
}
