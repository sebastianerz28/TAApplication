using System;
using System.Collections.Generic;
using System.Linq;
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

        public ApplicationsController(ILogger<ApplicationsController> logger, ApplicationDbContext DB,
            UserManager<TAUser> um)
        {
            _logger = logger;
            _context = DB;
            _um = um;
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

        [Authorize(Roles = "Admin, Professor, Applicant")]
        public async Task<IActionResult> Details(string? id)
        {
            /*if (_um.GetUserAsync(User).Result.Unid != id && (await _um.GetRolesAsync(_um.GetUserAsync(User).Result)).First().Equals("Applicant"))
            {
                return View("NotAuthorized");
            }*/
            if (id == null || _context.Applications == null)
            {
                return NotFound();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DegreePursuing,Department,GPA,DesiredHours,AvailableBeforeSemester,SemestersCompleted,PersonalStatement,TransferSchool,LinkedIn,ResumeName,CreationDate,ModificationDate")] Application application)
        {
            if (id != application.Id)
            {
                return NotFound();
            }
            ModelState.Remove("TAUser");


            //TODO: Fix this error
            //Issue: querying the user causes it to be tracked already which causes the error

            //Variation 1: causes error
            TAUser user = (await _context.Applications.Include("TAUser").FirstAsync(m => m.Id == id)).TAUser; //Gets user associated with application
            application.TAUser = user;


            //Variation 2 (Fixes it but this I assume this is not intended way,
            //as it defeats the purpose of Professor Germains tracking methods in ApplicationDbContext.cs)
            /* TAUser user = (await _context.Applications.AsNoTracking().Include("TAUser")
                 .FirstAsync(m => m.Id == id)).TAUser;
               application.TAUser = user;
               application.ModificationDate = DateTime.Now;
            */

            if (ModelState.IsValid)
            {
                try

                {
                    //For variation 2
                    /*_context.Entry(user).State = EntityState.Unchanged;*/
                    _context.Update(application);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = application.TAUser.Id });
            }
            return View(application);
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
    }
}
