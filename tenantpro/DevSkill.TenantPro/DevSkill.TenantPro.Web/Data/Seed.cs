using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevSkill.TenantPro.Web.Data
{
    public class Seed
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
           context.Database.EnsureCreated();
            

            string  roleAdmin = "Admin", roleUser = "Manager";
            string UserName = "shahedimamuddin@gmail.com", password = "shahed123";

            if (await roleManager.FindByNameAsync(roleAdmin) ==null)
                await roleManager.CreateAsync(new IdentityRole(roleAdmin));

            if (await roleManager.FindByNameAsync(roleUser) == null)
                await roleManager.CreateAsync(new IdentityRole(roleUser));

            if (await userManager.FindByNameAsync(UserName) ==null)
            {
                var user = new IdentityUser
                {
                    UserName= UserName,
                    Email = UserName,
                    PhoneNumber = "01777121212"
                };

                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await userManager.AddPasswordAsync(user, password);
                    await userManager.AddToRoleAsync(user, roleAdmin);
                }
                _ = user.Id;
            }

        }
    }
}
