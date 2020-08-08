using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TodoListApp.WebApp;

namespace TodoListApp.IntegrationTests
{
    public class StartupTest
    {
        private WebApplicationFactory<Startup> factory;

        [SetUp]
        public void Setup()
        {
            factory = new WebApplicationFactory<Startup>();
        }
        
        [TestCase("/Account/Login")]
        [TestCase("/Identity/Account/Login")]
        [TestCase("/TodoItems")]
        [TestCase("/")]
        public async Task ReturnsViewForUrl(string url)
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);
            
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("text/html; charset=utf-8"));
        }
    }
}