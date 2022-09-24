using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TAApplication.Data;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public class ApplicationRoleManager : RoleManager<TAUser>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole> store) : base(store.)
        {
            
        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>());
            return new ApplicationRoleManager(roleStore);
        }
    }
}