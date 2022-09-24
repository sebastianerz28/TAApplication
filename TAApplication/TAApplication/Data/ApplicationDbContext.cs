using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
            if (rm.Roles.Count() == 3)
            {
                //TODO: 
            }
        }
    }

    
}