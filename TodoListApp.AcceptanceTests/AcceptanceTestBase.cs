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
            // Not recommended to use flag AcceptInsecureCertificates. However, I decided to switch on the flag to save time. 
            Driver = new FirefoxDriver(new FirefoxOptions() { AcceptInsecureCertificates = true });
            Driver.Navigate().GoToUrl(LoginUrl);
            Driver.WaitUntilPageIsReady();

            AdditionalSetup();
        }

        protected void AuthenticateWithDefaultUser()
        {
            FillInput("UserName","test");
            FillInput("Password","pwd123");
            ClickOn("login");
            
            Driver.WaitUntilPageIsReady();
        }

        protected void FillInput(string id, string text)
        {
            Driver.FindElement(By.CssSelector($"input#{id}")).SendKeys($"{text}");
        }

        protected void ClickOn(string id)
        {
            Driver.FindElement(By.CssSelector($"button#{id}")).Submit();            
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