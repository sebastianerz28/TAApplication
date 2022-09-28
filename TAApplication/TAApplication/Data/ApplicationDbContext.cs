/**
 * Author:    Sebastian Ramirez
 * Partner:   Noah Carlson
 * Date:      09/27/2022
 * Course:    CS 4540, University of Utah, School of Computing
 * Copyright: CS 4540 and [Noah Carlson/Sebastian Ramirez] - This work may not be copied for use in Academic Coursework.
 *
 * I, Sebastian Ramirez, certify that I wrote this code from scratch and did 
 * not copy it in part or whole from another source.  Any references used 
 * in the completion of the assignment are cited in my README file and in
 * the appropriate method header.
 *
 * File Contents
 *      This File contains the seeding of the database.
 */

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TAApplication.Areas.Data;

namespace TAApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public async Task InitializeUsers(UserManager<TAUser> um, RoleManager<IdentityRole> rm)
        {
            if (rm.Roles.Count() != 3)
            {
                //Create new Roles
                await rm.CreateAsync(new IdentityRole("Admin"));
                await rm.CreateAsync(new IdentityRole("Professor"));
                await rm.CreateAsync(new IdentityRole("Applicant"));
            }

            if( um.Users.Count() < 5)
            {   //Create New user
                TAUser user = new TAUser { UserName = "admin@utah.edu", Unid = "u1234567",
                    Email = "admin@utah.edu", EmailConfirmed = true, Name = "Admin0", ReferredTo = "" };
                var result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Admin");
                }

                //Create New user
                user = new TAUser { UserName = "professor@utah.edu", Unid = "u7654321",
                    Email = "professor@utah.edu", EmailConfirmed = true, Name = "Prof0",  ReferredTo = "" };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Professor");
                }

                //Create New user
                user = new TAUser { UserName = "u0000000@utah.edu", Unid = "u0000000",
                    Email = "u0000000@utah.edu", EmailConfirmed = true, Name = "App0",  ReferredTo = "" };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    await um.AddClaimAsync(user, new Claim("UserApp0", user.Unid));
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Applicant");
                }

                //Create New user
                user = new TAUser { UserName = "u0000001@utah.edu", Unid = "u0000001",
                    Email = "u0000001@utah.edu", EmailConfirmed = true, Name = "App1",  ReferredTo = "" };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Applicant");
                }

                //Create New user
                user = new TAUser { UserName = "u0000002@utah.edu", Unid = "u0000002",
                    Email = "u0000002@utah.edu", EmailConfirmed = true, Name = "App2",  ReferredTo = "" };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Applicant");
                }
            }

        }
    }

    
}