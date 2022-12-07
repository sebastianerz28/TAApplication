/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      Sept 27, 2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and Noah Carlson/Sebastian Ramirez - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *
 *  This file serves as the controller for admins currently (9/27/2022) allowing them to add/remove roles.
 *    
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System;
using System.Data;
using System.Diagnostics;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Models;


namespace TAApplication.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        UserManager<TAUser> _um;
        RoleManager<IdentityRole> _rm;
        private ApplicationDbContext _context;
        public AdminController(ILogger<AdminController> logger, UserManager<TAUser> um, RoleManager<IdentityRole> rm, ApplicationDbContext DB)
        {
            _logger = logger;
            _um = um;
            _rm = rm;
            _context = DB;
        }

        /// <summary>
        /// Returns the View roles page
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns the View enrollment trends page
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult EnrollmentTrends()
        {
            return View();
        }


        /// <summary>
        /// This Method Changes the Role of the user
        /// </summary>
        /// <param name="user_id"> Id of user to be changed </param>
        /// <param name="role"> Role to assign/remove user to/from </param>
        /// <returns> Ok if user could have role updated, NotFound if there was an error </returns>
        [HttpPost]
        public async Task<IActionResult> Change_Role(string user_id, string role)
        {
            //Get user from the provided ID
            TAUser user = _um.FindByIdAsync(user_id).Result;
            if(!_um.IsInRoleAsync(user, role).Result)
            {
                //Add user to role if not in role
                var result = await _um.AddToRoleAsync(user, role);
                if(result.Succeeded)
                {
                    return Ok(new { success = true, message = "added!" });
                }
            }
            else 
            {
                //Remove user from role if in role
                var result = await _um.RemoveFromRoleAsync(user, role);
                if (result.Succeeded)
                {
                    return Ok(new { success = true, message = "removed!" });
                }
            }

            return NotFound(new { success = false, message = "Could not change"+ user.Name +" to " + role});
        }

        [HttpGet]
        public string GetEnrollmentData(string start, string end, string dept, string number)
        {
            var query = from e in _context.Enrollments
                        where e.Course == dept + " " + number && (e.LastUpdated >= DateTime.Parse(start) && e.LastUpdated <= DateTime.Parse(end))
                        
                        select e;
            if(query.Any())
            {
                return query.ToList().ToJson();
            }
            return "";
        }

    }
}
