using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Data
{
    public static class SeedData
    {
        public async static Task Seed(UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
           
        }
        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            //check if this email is exist.
            if(await userManager.FindByEmailAsync("admin@bookstore.com") == null)
            {
                //set username and email which will use with CreateAsync Function below.
                var user = new IdentityUser
                {
                    UserName = "admin@bookstore.com",
                    Email = "admin@bookstore.com"
                };
                //create a new user with given detail.
                var result = await userManager.CreateAsync(user, "$Lovr08155605");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administration");
                }
            }

            if (await userManager.FindByEmailAsync("customer@bookstore.com") == null)
            {
                //set username and email which will use with CreateAsync Function below.
                var user = new IdentityUser
                {
                    UserName = "customer@bookstore.com",
                    Email = "customer@bookstore.com"
                };
                //create a new user with given detail.
                var result = await userManager.CreateAsync(user, "$Lovr08155605");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");
                }
            }
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administration"))
            {
                var role = new IdentityRole
                {
                    Name = "Administration"
                };
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                var role = new IdentityRole
                {
                    Name = "Customer"
                };
                await roleManager.CreateAsync(role);
            }
        }
    }
}
