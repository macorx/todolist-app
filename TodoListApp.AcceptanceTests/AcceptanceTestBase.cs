using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TodoListApp.AcceptanceTests
{
    public class AcceptanceTestBase
    {
        protected IWebDriver Driver { get; private set; }

        protected const string BasePath = "https://localhost:5001";
        protected readonly string LoginUrl = $"{BasePath}/Identity/Account/Login";

        [SetUp]
        public void Setup()
        {
            Driver = new FirefoxDriver(new FirefoxOptions() { AcceptInsecureCertificates = true });
            Driver.Navigate().GoToUrl(LoginUrl);
            Driver.WaitUntilPageIsReady();            
        }
        
        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
            Driver.Dispose();
        }

    }
}