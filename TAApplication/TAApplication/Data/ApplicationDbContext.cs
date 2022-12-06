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

using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TAApplication.Areas.Data;
using TAApplication.Models;
using static System.Net.Mime.MediaTypeNames;
using Application = TAApplication.Models.Application;

namespace TAApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<TAUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbSet<Application> Applications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor http)
            : base(options)
        {
            _httpContextAccessor = http;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Application>().ToTable("Applications");
            modelBuilder.Entity<Availability>().ToTable("Availabilities");
            modelBuilder.Entity<Course>().ToTable("Courses");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollments");
        }

        /// <summary>
        /// Seeds the Users database with 5 users with 3 different roles
        /// </summary>
        public async Task InitializeUsers(UserManager<TAUser> um, RoleManager<IdentityRole> rm)
        {
            if (rm.Roles.Count() != 3)
            {
                //Create new Roles
                await rm.CreateAsync(new IdentityRole("Admin"));
                await rm.CreateAsync(new IdentityRole("Professor"));
                await rm.CreateAsync(new IdentityRole("Applicant"));
            }

            if (um.Users.Count() < 5)
            {   //Create New user
                TAUser user = new()
                {
                    UserName = "admin@utah.edu",
                    Unid = "u1234567",
                    Email = "admin@utah.edu",
                    EmailConfirmed = true,
                    Name = "Admin0",
                    ReferredTo = ""
                };
                var result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {

                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Admin");
                }

                //Create New user
                user = new TAUser
                {
                    UserName = "professor@utah.edu",
                    Unid = "u7654321",
                    Email = "professor@utah.edu",
                    EmailConfirmed = true,
                    Name = "Prof0",
                    ReferredTo = ""
                };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Professor");
                }

                //Create New user
                user = new TAUser
                {
                    UserName = "u0000000@utah.edu",
                    Unid = "u0000000",
                    Email = "u0000000@utah.edu",
                    EmailConfirmed = true,
                    Name = "App0",
                    ReferredTo = ""
                };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Applicant");
                }

                //Create New user
                user = new TAUser
                {
                    UserName = "u0000001@utah.edu",
                    Unid = "u0000001",
                    Email = "u0000001@utah.edu",
                    EmailConfirmed = true,
                    Name = "App1",
                    ReferredTo = ""
                };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Applicant");
                }

                //Create New user
                user = new TAUser
                {
                    UserName = "u0000002@utah.edu",
                    Unid = "u0000002",
                    Email = "u0000002@utah.edu",
                    EmailConfirmed = true,
                    Name = "App2",
                    ReferredTo = ""
                };
                result = await um.CreateAsync(user, "123ABC!@#def");
                if (result.Succeeded)
                {
                    //add this to add role to user
                    await um.AddToRoleAsync(user, "Applicant");
                }
            }



        }

        /// <summary>
        /// Seeds the Courses database with 5 courses
        /// </summary>
        /// <returns></returns>
        public async Task InitializeCourses()
        {
            if (Courses.Count() < 5)
            {
                Course c1 = new Course
                {
                    SemesterOffered = "spring",
                    YearOffered = "2023",
                    Title = "Intro to Object Oriented Programming",
                    Department = "CS",
                    Number = "1400",
                    Section = "001",
                    Description = "This course is an introduction to the engineering and mathematical skills required to effectively program computers and is designed for students with no prior programming experience.",
                    ProfessorUnid = "U0123729",
                    ProfessorName = "David Johnson",
                    Start = TimeSpan.Parse("01:25"),
                    End = TimeSpan.Parse("02:45"),
                    DaysOffered = "Mo/We",
                    Location = "S BEH AUD",
                    CreditHours = 4,
                    Enrollment = 0,
                    Note = "This course needs more TAs"
                };
                Courses.Add(c1);

                Course c2 = new Course
                {
                    SemesterOffered = "Spring",
                    YearOffered = "2023",
                    Title = "Data Structures",
                    Department = "CS",
                    Number = "2420",
                    Section = "001",
                    Description = "This course provides an introduction to the problem of engineering computational efficiency into programs. Students will learn about classical algorithms (including sorting, searching, and graph traversal), data structures (including stacks, queues, linked lists, trees, hash tables, and graphs), and analysis of program space and time requirements. Students will complete extensive programming exercises that require the application of elementary techniques from software engineering.",
                    ProfessorUnid = "u0834567",
                    ProfessorName = "Erin Parker",
                    Start = TimeSpan.Parse("08:00"),
                    End = TimeSpan.Parse("09:00"),
                    DaysOffered = "Tu/Th",
                    Location = "S BEH AUD",
                    CreditHours = 4,
                    Enrollment = 200,
                };
                Courses.Add(c2);

                Course c3 = new Course
                {
                    SemesterOffered = "Spring",
                    YearOffered = "2023",
                    Title = "Software Practice I",
                    Department = "CS",
                    Number = "3500",
                    Section = "001",
                    Description = "Practical exposure to the process of creating large software systems, including requirements specifications, design, implementation, testing, and maintenance. Emphasis on software process, software tools (debuggers, profilers, source code repositories, test harnesses), software engineering techniques (time management, code, and documentation standards, source code management, object-oriented analysis and design), and team development practice. Much of the work will be in groups and will involve modifying preexisting software systems.",
                    ProfessorUnid = "u0834563",
                    ProfessorName = "Daniel Kopta",
                    Start = TimeSpan.Parse("10:00"),
                    End = TimeSpan.Parse("11:00"),
                    DaysOffered = "Tu/Th",
                    Location = "WEB L105",
                    CreditHours = 4,
                    Enrollment = 140,
                };
                Courses.Add(c3);

                Course c4 = new Course
                {
                    SemesterOffered = "Spring",
                    YearOffered = "2023",
                    Title = "Software Practice II",
                    Department = "CS",
                    Number = "3505",
                    Section = "001",
                    Description = "An in-depth study of traditional software development (using UML) from inception through implementation. The entire class is team-based, and will include a project that uses an agile process.",
                    ProfessorUnid = "u0935641",
                    ProfessorName = "David Johnson",
                    Start = TimeSpan.Parse("12:25"),
                    End = TimeSpan.Parse("01:45"),
                    DaysOffered = "Tu/Th",
                    Location = "WEB L104",
                    CreditHours = 3,
                    Enrollment = 50,
                };
                Courses.Add(c4);

                Course c5 = new Course
                {
                    SemesterOffered = "Spring",
                    YearOffered = "2023",
                    Title = "Computer Systems",
                    Department = "CS",
                    Number = "4400",
                    Section = "001",
                    Location = "FMAB AUD",
                    Description = "Introduction to computer systems from a programmer's point of view.  Machine level representations of programs, optimizing program performance, memory hierarchy, linking, exceptional control flow, measuring program performance, virtual memory, concurrent programming with threads, network programming.",
                    ProfessorUnid = "u0834563",
                    ProfessorName = "Mu Zhang",
                    Start = TimeSpan.Parse("11:50"),
                    End = TimeSpan.Parse("1:10"),
                    DaysOffered = "M/W",
                    CreditHours = 4,
                    Enrollment = 100,
                    Note = "This course requires 6 TAs"
                };
                Courses.Add(c5);

                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Seeds the Applications database with 2 applications
        /// </summary>
        /// <returns></returns>
        public async Task InitializeApplications(UserManager<TAUser> um)
        {
            if (Applications.Count() < 2)
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
                    ResumeName = "siduygawgv.pdf",
                    ProfilePicName = "sjadbalibda.png",
                    AvailableBeforeSemester = true,
                    SemestersCompleted = 3,
                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now,
                    TAUser = um.FindByEmailAsync("u0000001@utah.edu").Result


                };

                Applications.Add(a1);
                await SaveChangesAsync();
            }


        }

        public async Task InitAvailability(UserManager<TAUser> um)
        {
            if (Availabilities.Count() == 0)
            {

                Availability a = new()
                {
                    Monday = "1111111111111111000000000000000000000000000000000000000000000000",
                    Tuesday = "0000000000000000111111111111111111110000000000000000000000000000",
                    Wednesday = "0000000000000000000000000000000000000000000000000000000000000000",
                    Thursday = "0000000000000000111111111111111111110000000000000000000000000000",
                    Friday = "1111111111111111000000000000000000000000000000000000000000000000",
                    TAUser = um.FindByEmailAsync("u0000000@utah.edu").Result
                };
                Availabilities.Add(a);
                await SaveChangesAsync();
            }
        }

        public async Task InitEnrollments()
        {
            if (Enrollments.Count() == 0)
            {
                using (var reader = new StreamReader("../TAApplication/wwwroot/temp.csv"))
                {
                    string? line = reader.ReadLine();
                    string[] dates = line.Split(',');
                    line = reader.ReadLine();
                    while (line != null)
                    {
                        string[] vals = line.Split(',');
                        for (int i = 1; i < vals.Length; i++)
                        {
                            Enrollment e = new Enrollment()
                            {
                                Course = vals[0],
                                LastUpdated = DateTime.Parse(dates[i]),
                                TotalEnrollment = int.Parse(vals[i])
                            };
                            Enrollments.Add(e);
                            await SaveChangesAsync();
                        }

                        line = reader.ReadLine();
                    }
                }
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
            //Gets entries that were modified
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is ModificationTracking
                        && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = "";
            //If database is being seeded the Database is modifying
            if (_httpContextAccessor.HttpContext == null) // happens during startup/initialization code
            {
                currentUsername = "DBSeeder";
            }
            //If a user is changing get the username
            else
            {
                currentUsername = _httpContextAccessor.HttpContext.User.Identity?.Name;
            }
            //IF a user is not logged in
            currentUsername ??= "Sadness"; // JIM: compound assignment magic... test for null, and if so, assign value
            //Loop over all entries
            foreach (var entity in entities)
            {
                //If entry is being created set the Createed values
                if (entity.State == EntityState.Added)
                {
                    ((ModificationTracking)entity.Entity).CreationDate = DateTime.UtcNow;
                    ((ModificationTracking)entity.Entity).CreatedBy = currentUsername;
                }
                //Set Modification dates
                ((ModificationTracking)entity.Entity).ModificationDate = DateTime.UtcNow;
                ((ModificationTracking)entity.Entity).ModifiedBy = currentUsername;
            }
        }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        public DbSet<TAApplication.Models.Course> Course { get; set; }
    }


}