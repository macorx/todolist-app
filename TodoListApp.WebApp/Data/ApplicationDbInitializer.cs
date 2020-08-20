using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApp.Data
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string userRole = "User";
            const string adminRole = "Admin";
            
            await AddRole(roleManager, userRole);
            await AddRole(roleManager, adminRole);
            
            await AddUser(userManager, "admin", "pwd123", adminRole);
            await AddUser(userManager, "test", "pwd123", userRole);
            await AddUser(userManager, "test2", "pwd123", userRole);
        }

        private static async Task AddRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) != null)
                return;
            
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        private static async Task AddUser(UserManager<IdentityUser> userManager, string userName, string password,
            string role)
        {
            if (await userManager.FindByNameAsync(userName) != null)
                return;
            
            var user = new IdentityUser {UserName = userName};
            await userManager.CreateAsync(user, password);
            // ignoring identity result checking since users are hardcoded.

            await userManager.AddToRoleAsync(user, role);
        }
    }
}