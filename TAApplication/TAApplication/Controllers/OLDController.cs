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
 *  This file serves as the controller for the home page returning appropriate views for other pages
 *    
 */

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
    public class OLDController : Controller
    {
        private readonly ILogger<OLDController> _logger;
        UserManager<TAUser> _um;

        public OLDController(ILogger<OLDController> logger, UserManager<TAUser> um)
        {
            _logger = logger;
            _um = um;
        }

        /// <summary>
        /// Returns the home page
        /// Accessible by any user regardless of login status
        /// </summary>
        /// <returns> Home Page </returns>

        /// <summary>
        /// Returns the privacy view
        /// Only accessible by logged in user
        /// </summary>
        /// <returns> Privacy Page </returns

        /// <summary>
        /// Returns the page to create an application
        /// Only Accessible by Applicants
        /// </summary>
        /// <returns> Application Create Page </returns>
        public IActionResult ApplicationCreate()
        {
            return View();
        }

        /// <summary>
        /// Returns the Applicant List
        /// Only Accesible by professors and admins
        /// </summary>
        /// <returns> ApplicantList Page</returns
        public IActionResult ApplicantList()
        {
            return View();
        }


        /// <summary>
        /// Returns Application Details
        /// Only Accesible by professors, admins, and specific user u0000000
        /// </summary>
        /// <returns> ApplicationDetails Page </returns>
        
        public IActionResult ApplicationDetails()
        {
            //Check if it is user u0000000
            /*
            if (_um.GetUserAsync(User).Result.Unid != "u0000000" && _um.GetRolesAsync(_um.GetUserAsync(User).Result).Result.FirstOrDefault().Equals("Applicant"))
            {
                return View("NotAuthorized");
            }
            else
            {
                return View();
            }
            */
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}