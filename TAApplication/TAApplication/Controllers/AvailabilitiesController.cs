using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    [Authorize]
    public class AvailabilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvailabilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Availabilities
        
        public async Task<IActionResult> Index()
        {
              return View();
        }

        [HttpPost]
        [Authorize(Roles = "Applicant")]
        public async Task SetSchedule(string[] slots)
        {

        }
        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public Availability GetSchedule(string id)
        {
            return _context.Availabilities.Include("TAUser").Where(c => c.TAUser.Unid == id).First();
        }


        private bool AvailabilityExists(int id)
        {
          return _context.Availabilities.Any(e => e.Id == id);
        }
    }
}
