using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        protected IWebDriver Driver => WebDriver.Instance;

        private const string BasePath = "https://localhost:5001";
        protected readonly string LoginUrl = $"{BasePath}/Account/Login";
        
        protected TodoListPage SignIn()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            
            return new SignInPage(Driver)
                .WithUserName("test")
                .WithPassword("pwd123")
                .SignIn<TodoListPage>();
        }

    }
}