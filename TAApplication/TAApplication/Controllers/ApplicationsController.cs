/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      10/8/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and [Your Name(s)] - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez and Noah Carlson, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *    This file serves as the controller to applications gets and retrieves data from the database relating to applications
 */

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

        /// <summary>
        /// Get the list of all applicants.
        /// Only accesible to admins and professors
        /// </summary>
        /// <returns>Returns the view along with the data of the applicants</returns>
        [Authorize(Roles = "Admin, Professor")]
        public async Task<IActionResult> List()
        {
            return View(await _context.Applications.Include("TAUser").ToArrayAsync());
        }

        // GET: Applications
        /// <summary>
        /// Gets the home page for Applications
        /// Only accesible to admins
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var gpaQuery = from a in _context.Applications
                           select a.GPA;
            if (gpaQuery.Any())
            {
                double gpaAvg = 0;
                foreach (var item in gpaQuery)
                    gpaAvg += item;
                gpaAvg = gpaAvg / gpaQuery.Count();
                ViewData["gpaAvg"]=gpaAvg;

            }
            return View(await _context.Applications.Include("TAUser").ToArrayAsync());
        }
        // GET: Details
        /// <summary>
        /// Gets the details to an application
        /// </summary>
        /// <param name="id">ID of the application</param>
        /// <returns>returns the view and application to be displayed</returns>
        [Authorize(Roles = "Admin, Professor, Applicant")]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            if (id != _um.GetUserAsync(User).Result.Id && !_um.IsInRoleAsync(_um.GetUserAsync(User).Result,"Admin").Result)
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

        // GET: Applications/Create
        /// <summary>
        /// Gets the page to create an application
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Creates an application and stores it in the Database
        /// </summary>
        /// <param name="files"></param>
        /// <param name="application"> Application object created from user input </param>
        /// <returns>Redirects to details if creation was a success shows error otherwise</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files, [Bind("Id,DegreePursuing,Department,GPA,DesiredHours,AvailableBeforeSemester,SemestersCompleted,PersonalStatement,TransferSchool,LinkedIn,ResumeName,ProfilePicName,CreationDate,ModificationDate")] Application application)
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
                    var appID = from a in _context.Applications.Include("TAUser")
                                where a.TAUser.Id == user.Id
                                select a.Id;
                    if (application.ProfilePicName != null)
                    {
                        await FileUpload(files, "resume", appID.First());
                    }
                    if (application.ResumeName != null)
                    {
                        await FileUpload(files, "photo", appID.First());
                    }

                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }
            }
            //If application exists redirects to the details of the application
            return RedirectToAction(nameof(Details), new { id = query.First().TAUser.Id });
            /*return View(application);*/
        }

        // GET: Applications/Edit/5
        /// <summary>
        /// Gets the application to be edited 
        /// </summary>
        /// <param name="id">ID of the Application</param>
        /// <returns></returns>
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
        /// <summary>
        /// Edits the application based on the updated user input
        /// </summary>
        /// <param name="id">ID of the application</param>
        /// <param name="application">Application model object created from user input</param>
        /// <returns>Redirects to details if success, bad request otherwise</returns>
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
        /// <summary>
        /// Gets the delete page of the specified applicaiton
        /// </summary>
        /// <param name="id">ID of the application</param>
        /// <returns>The delete page with the application fields, NotFound if the no ID matching the provided ID could be found</returns>
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
        /// <summary>
        /// Deletes an application from the Database
        /// </summary>
        /// <param name="id">ID of the application to be deleted</param>
        /// <returns> Redirects to homepage of website </returns>
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
            return RedirectToAction("Index", "Home");
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
        /// <summary>
        /// Uploads a file to the server and stores the name in the databse
        /// </summary>
        /// <param name="files">File to be uploaded</param>
        /// <param name="category">Category of the file</param>
        /// <param name="appID">ID of the application to associate the file with</param>
        /// <returns>Redirects to details if success, returns bad request otherwise</returns>
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
