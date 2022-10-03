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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TAApplication.Areas.Data;
using TAApplication.Models;

namespace TAApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        private HttpContextAccessor _httpContextAccessor;
        public DbSet<Application> Applications { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            HttpContextAccessor http)
            : base(options)
        {
            _httpContextAccessor = http;
        }

        public ApplicationDbContext()
        {
            
            _httpContextAccessor = new HttpContextAccessor();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Application>().ToTable("Applications");
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

            if(Applications.Count() < 2)
            {
                Application a0 = new Application
                {

                    DegreePursuing = DegreePursuing.BS,
                    Department = "CS",
                    GPA = 3.0,
                    DesiredHours = 10,
                    AvailableBeforeSemester = true,
                    SemestersCompleted = 3,
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now,
                    TAUser = um.FindByEmailAsync("u0000000@utah.edu").Result


                };

                Applications.Add(a0);
                SaveChanges();




                Application a1 = new Application
                {

                    DegreePursuing = DegreePursuing.BS,
                    Department = "CS",
                    GPA = 3.0,
                    DesiredHours = 10,
                    PersonalStatement = "Sample Personal Statement",
                    TransferSchool = "Arizona State",
                    LinkedIn = "www.linkedin.com",
                    ResumeName = "app1resume.pdf",
                    AvailableBeforeSemester = true,
                    SemestersCompleted = 3,
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now,
                    TAUser = um.FindByEmailAsync("u0000001@utah.edu").Result


                };

                Applications.Add(a1);
                SaveChanges();
            }



        }

        /// <summary>
        /// Every time Save Changes is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()  // JIM: Override save changes to add timestamps
        {
            AddTimestamps();
            return base.SaveChanges();
        }
        /// <summary>
        /// Every time Save Changes (Async) is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            AddTimestamps();   // JIM: Override save changes async to add timestamps
            return await base.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is ModificationTracking
                        && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = "";

            if (_httpContextAccessor.HttpContext == null) // happens during startup/initialization code
            {
                currentUsername = "DBSeeder";
            }
            else
            {
                currentUsername = _httpContextAccessor.HttpContext.User.Identity?.Name;
            }

            currentUsername ??= "Sadness"; // JIM: compound assignment magic... test for null, and if so, assign value

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((ModificationTracking)entity.Entity).CreationDate = DateTime.UtcNow;
                    ((ModificationTracking)entity.Entity).CreatedBy = currentUsername;
                }
                ((ModificationTracking)entity.Entity).ModificationDate = DateTime.UtcNow;
                ((ModificationTracking)entity.Entity).ModifiedBy = currentUsername;
            }
        }
    }

    
}