using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace TodoListApp.IntegrationTests
{
    public class ApplicationDbInitializerTest : IntegrationTestBase
    {
        private UserManager<IdentityUser> userManager;

        protected override void AdditionalSetup()
        {
            userManager = ServiceProvider.GetService<UserManager<IdentityUser>>();
        }

        [TestCase("test")]
        [TestCase("test2")]
        [TestCase("admin")]
        public async Task UserExists(string userName)
        {
            Assert.IsNotNull(await userManager.FindByNameAsync(userName));
        }
        
        [TestCase("test", "User")]
        [TestCase("test2", "User")]
        [TestCase("admin", "Admin")]
        public async Task UserIsInRole(string userName, string roleName)
        {
            var user = await userManager.FindByNameAsync(userName);
            Assert.IsTrue(await userManager.IsInRoleAsync(user, roleName));
        }
    }
}