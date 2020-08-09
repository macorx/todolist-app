using Microsoft.AspNetCore.Identity;

namespace TodoListApp.WebApp.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string userRole = "User";
            
            AddRole(roleManager, userRole);
            
            AddUser(userManager, "test", "pwd123", userRole);
            AddUser(userManager, "test2", "pwd123", userRole);
        }

        private static void AddRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
        }

        private static void AddUser(UserManager<IdentityUser> userManager, string userName, string password,
            string role)
        {
            var user = new IdentityUser {UserName = userName};
            userManager.CreateAsync(user, password).Wait();

            // ignoring identity result checking since users are hardcoded.

            userManager.AddToRoleAsync(user, role).Wait();
        }
    }
}