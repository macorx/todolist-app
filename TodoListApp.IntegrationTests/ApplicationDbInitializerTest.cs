using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TodoListApp.WebApp;

namespace TodoListApp.IntegrationTests
{
    public class ApplicationDbInitializerTest
    {
        private WebApplicationFactory<Startup> factory;
        private UserManager<IdentityUser> userManager;
        private IServiceScope scope;

        [SetUp]
        public void Setup()
        {
            factory = new WebApplicationFactory<Startup>();

            scope = factory.Services.CreateScope();
            userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        }

        [TearDown]
        public void TearDown()
        {
            scope.Dispose();
        }

        [TestCase("test")]
        [TestCase("test2")]
        public async Task UserExists(string userName)
        {
            Assert.IsNotNull(await userManager.FindByNameAsync(userName));
        }
        
        [TestCase("test", "User")]
        [TestCase("test2", "User")]
        public async Task UserIsInRole(string userName, string roleName)
        {
            var user = await userManager.FindByNameAsync(userName);
            Assert.IsTrue(await userManager.IsInRoleAsync(user, roleName));
        }
    }
}