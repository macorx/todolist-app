using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        protected IWebDriver Driver { get; private set; }

        private const string BasePath = "https://localhost:5001";
        protected readonly string LoginUrl = $"{BasePath}/Identity/Account/Login";

        [SetUp]
        public void Setup()
        {
            var service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";

            // Not recommended to use flag AcceptInsecureCertificates. However, I decided to switch on the flag to save time.
            var options = new FirefoxOptions() {  AcceptInsecureCertificates = true };

            Driver = new FirefoxDriver(service, options);
            Driver.Navigate().GoToUrl(LoginUrl);
            Driver.WaitUntilPageIsReady();
            
            AdditionalSetup();
        }

        protected TodoListPage SignIn()
        {
            return new SignInPage(Driver)
                .WithUserName("test")
                .WithPassword("pwd123")
                .SignIn();
        }

        protected virtual void AdditionalSetup()
        {
        }

        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
            Driver.Dispose();
        }

    }
}