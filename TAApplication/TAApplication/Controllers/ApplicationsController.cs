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

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
                var query = from a in _context.Applications where a.TAUser == user select a.Id;
                return RedirectToAction(nameof(Details),new { id = query.First() });
            }
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            return View(application);
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

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
