/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      11/22/2022
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
 *    This file serves as the controller for availability
 *    Provides end points to get and set a users/availability
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Sends Index Page
        /// </summary>
        /// <returns></returns>
        [Authorize (Roles = "Applicant")]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        /// <summary>
        /// Receives slots and a userID, updates the database accordingly
        /// </summary>
        /// <param name="slots">A string containing 240 1s or 0s</param>
        /// <param name="userId">Id of the user</param>
        /// <returns>Ok if the database could be updated successfully, BadRequest Otherwise</returns>
        [HttpPost]
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> SetSchedule(string slots, string userId)
        {
            if (AvailabilityExists(userId) && slots.Length == 240)
            {
                var result = _context.Availabilities.Include("TAUser").First(c => c.TAUser.Id == userId);
                string[] days = new string[5];
                for (int i = 0; i < slots.Length; i += 48)
                {
                    days[i / 48] = slots.Substring(i, 48);

                }


                result.Monday = days[0];
                result.Tuesday = days[1];
                result.Wednesday = days[2];
                result.Thursday = days[3];
                result.Friday = days[4];


                _context.Availabilities.Update(result);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "added!" });
            }
            else
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Gets the schedule for a user given a userId
        /// </summary>
        /// <param name="userid">ID of the user</param>
        /// <returns>Existing availability, or a new one if the user does not exist</returns>
        [HttpGet]
        [Authorize(Roles = "Applicant")]
        public string GetSchedule(string userid)
        {
            if (AvailabilityExists(userid))
            {
                var result = _context.Availabilities.Include("TAUser").First(c => c.TAUser.Id == userid);
                var days = new
                {
                    result.Monday,
                    result.Tuesday,
                    result.Wednesday,
                    result.Thursday,
                    result.Friday
                };
                return days.ToJson();
            }
            else
            {
                Availability a = new Availability();
                StringBuilder emptySlotStringBuilder = new StringBuilder();
                for (int i = 0; i < 48; i++)
                {
                    emptySlotStringBuilder.Append('0');
                }

                a.Monday = emptySlotStringBuilder.ToString();
                a.Tuesday = emptySlotStringBuilder.ToString();
                a.Wednesday = emptySlotStringBuilder.ToString();
                a.Thursday = emptySlotStringBuilder.ToString();
                a.Friday = emptySlotStringBuilder.ToString();
                a.TAUser = _userManager.FindByIdAsync(userid).Result;
                _context.Availabilities.Add(a);
                _context.SaveChanges();

                return new
                {
                    a.Monday,
                    a.Tuesday,
                    a.Wednesday,
                    a.Thursday,
                    a.Friday
                }.ToJson();
            }
        }


        private bool AvailabilityExists(string userid)
        {
            var temp = _context.Availabilities.Include("TAUser").Any(e => e.TAUser.Id == userid);
            return temp;
        }
    }
}
