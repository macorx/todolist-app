using NUnit.Framework;
using NUnit.Framework.Interfaces;
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

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var outcome = TestContext.CurrentContext.Result.Outcome;
            if (outcome == ResultState.Failure || outcome == ResultState.Error)
                Driver.TakeScreenshot(testName);
            
            AdditionalTearDown();
        }

        protected virtual void AdditionalTearDown()
        {
        }
    }
}