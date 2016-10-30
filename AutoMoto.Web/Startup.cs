using AutoMoto.Model.Models;
using AutoMoto.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutoMoto.Web.Startup))]
namespace AutoMoto.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SeedData();
        }

        private void SeedData()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists(Roles.Admin))
            {
                var role = new IdentityRole { Name = Roles.Admin };
                roleManager.Create(role);

                var user = new ApplicationUser
                {
                    UserName = "admin@domain.com",
                    Email = "admin@domain.com"
                };

                string userPwd = "@dmin123";

                var userResult = userManager.Create(user, userPwd);

                if (userResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, Roles.Admin);

                }
            }

            if (!roleManager.RoleExists(Roles.Employee))
            {
                var role = new IdentityRole { Name = Roles.Employee };
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(Roles.Customer))
            {
                var role = new IdentityRole { Name = Roles.Customer };
                roleManager.Create(role);
            }
        }
    }
}
