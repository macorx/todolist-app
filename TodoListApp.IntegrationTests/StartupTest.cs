using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TodoListApp.WebApp;

namespace TodoListApp.IntegrationTests
{
    public class StartupTest : IntegrationTestBase
    {
        [TestCase("/Account/Login")]
        [TestCase("/Identity/Account/Login")]
        [TestCase("/")]
        [TestCase("/TodoItems")]
        [TestCase("/TodoItems/Add")]
        [TestCase("/TodoItems/Edit")]
        public async Task ReturnsViewForUrl(string url)
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync(url);
            
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("text/html; charset=utf-8"));
        }
    }
}