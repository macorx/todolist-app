using OpenQA.Selenium;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        protected IWebDriver Driver => WebDriver.Instance;

        private const string BasePath = "https://localhost:5001";
        protected readonly string LoginUrl = $"{BasePath}/Account/Login";
        
        protected TodoListPage SignIn(string userName = "test", string password= "pwd123")
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            
            return new SignInPage(Driver)
                .WithUserName(userName)
                .WithPassword(password)
                .SignIn<TodoListPage>();
        }

    }
}