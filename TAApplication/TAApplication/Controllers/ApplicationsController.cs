using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TAApplication.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly ILogger<ApplicationsController> _logger;
        private UserManager<TAUser> _um;
        IConfiguration _configuration;


        public ApplicationsController(ILogger<ApplicationsController> logger, ApplicationDbContext DB,
            UserManager<TAUser> um, IConfiguration config)
        {
            _logger = logger;
            _context = DB;
            _um = um;
            _configuration = config;
        }

        // GET: Applications
        [Authorize(Roles = "Admin, Professor")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Applications.Include("TAUser").ToArrayAsync());
        }

        // GET: Applications
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Applications.Include("TAUser").ToArrayAsync());
        }
        // GET: Details
        [Authorize(Roles = "Admin, Professor, Applicant")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            if(id != _um.GetUserAsync(User).Result.Id)
            {
                return View("NotAuthorized");
            }


            var application = from a in _context.Applications.Include("TAUser")
                              join u in _context.Users on a.TAUser equals u
                              where u.Id == id
                              select a;


            /* var application = await _context.Applications
                 .FirstOrDefaultAsync(m => m.TAUser.Id.Equals(id));*/

            if (application.Count() == 0)
            {
                return NotFound();
            }

            return View(await application.FirstOrDefaultAsync());
        }

        // GET: Applications/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Applications == null)
        //    {
        //        return NotFound();
        //    }

        //    var application = await _context.Applications
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (application == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(application);
        //}

        // GET: Applications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DegreePursuing,Department,GPA,DesiredHours,AvailableBeforeSemester,SemestersCompleted,PersonalStatement,TransferSchool,LinkedIn,ResumeName,CreationDate,ModificationDate")] Application application)
        {

            //Finds application matching with matching userID
            var query = from a in _context.Applications.Include("TAUser")
                        where a.TAUser.Id == _um.GetUserId(User)
                        select a;
            //If query has no results then an application for this user has not been made
            if (query.Count() == 0)
            {
                ModelState.Remove("TAUser");
                TAUser user = await _um.GetUserAsync(User);
                application.TAUser = user;
                if (ModelState.IsValid)
                {
                    var date = DateTime.Now;
                    application.CreationDate = date;
                    application.ModificationDate = date;
                    _context.Add(application);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }
            }
            //If application exists redirects to the details of the application
            return RedirectToAction(nameof(Details), new { id = query.First().TAUser.Id });
            /*return View(application);*/
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }


            var query = from a in _context.Applications.Include("TAUser")
                        where a.Id == id
                        select a;
            if (query.Count() < 1)
            {
                return NotFound();
            }
            return View(await query.FirstOrDefaultAsync());
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Applicant")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,DegreePursuing,Department,GPA,DesiredHours,AvailableBeforeSemester,SemestersCompleted,PersonalStatement,TransferSchool,LinkedIn,ResumeName,CreationDate,ModificationDate")] Application application)
        {
            if (id == null) { return BadRequest(); }
            var applicationToUpdate = _context.Applications
                                    .Where(o => o.Id == id).Include(o => o.TAUser).FirstOrDefault();
            if (applicationToUpdate != null)
            {
                ModelState.Remove("TAUser");
                TAUser user = await _um.GetUserAsync(User);
                applicationToUpdate.TAUser = user;

                if (await TryUpdateModelAsync<Application>(applicationToUpdate, "",
                    s => s.DegreePursuing,
                    s => s.Department,
                    s => s.GPA,
                    s => s.DesiredHours,
                    s => s.AvailableBeforeSemester,
                    s => s.SemestersCompleted,
                    s => s.PersonalStatement,
                    s => s.TransferSchool,
                    s => s.LinkedIn,
                    s => s.ResumeName))
                {
                    try
                    {
                        _context.SaveChanges();
                        return RedirectToAction(nameof(Details), new { id = applicationToUpdate.TAUser.Id });
                    }
                    catch (DataException)
                    {
                        // manage error logging
                    }
                    return View(applicationToUpdate);
                }
            }
            return BadRequest();
        }

        // GET: Applications/Delete/5
        [Authorize(Roles = "Admin, Applicant")]
        public async Task<IActionResult> Delete(int? id)
        {

            var query = from a in _context.Applications.Include("TAUser")
                        where a.Id == id
                        select new { id = a.TAUser.Id };

            if ((_um.GetUserId(User) != query.First().id) && User.IsInRole("Applicant"))
            {
                return View("NotAuthorized");
            }

            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);




        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Applications == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Applications'  is null.");
            }
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FileUpload(List<IFormFile> files, string category, int appID)
        {
            var application = await _context.Applications
                .FirstOrDefaultAsync(a => a.Id == appID);
            var user = _um.GetUserAsync(User).Result;

            //Check if there is an application
            if (application == null)
            {
                return BadRequest();
            }

            //Check if user id's match or if user is an admin
            if (application.TAUser.Id != user.Id && !_um.IsInRoleAsync(user, "admin").Result)
            {
                ViewData["ErrorMessage"] = "Must be Authorized to upload this file to this PAGE!";
                return View("Details", application);
            }

            if (files.Count != 1)
            {
                ViewData["ErrorMessage"] = "Please choose one file!";
                return View("Details", application);
            }

            // Check file size
            var file = files.First();
            int file_size = 10000000;
            if (file.Length > file_size || file.Length <= 0)
            {
                ViewData["ErrorMessage"] = "Please check your file size it may be too big or too small!";
                return View("Details", application);
            }

            //Create New FileName and Path
            string new_filename;
            do
            {
                new_filename = Path.GetRandomFileName();
                new_filename = new_filename.Remove(new_filename.IndexOf('.'));
                new_filename += file.FileName.Substring(file.FileName.IndexOf('.'));
            }
            while (_context.Applications.Any(e => e.ResumeName != null && e.ResumeName.Equals(new_filename)));
            string path = Path.Combine(_configuration["FilePath"], new_filename);


            // Save File
            if (category.Equals("resume"))
            {
                string pattern = "(.*\\.)(pdf)$";
                if (Regex.IsMatch(file.FileName, pattern))
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    application.ResumeName = new_filename;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }

            }
            else if(category.Equals("photo"))
            {
                string pattern = "(.*\\.)(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$";
                if (Regex.IsMatch(file.FileName, pattern))
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    application.ProfilePicName = new_filename;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }

            }

            return BadRequest();
        }

    }
}
