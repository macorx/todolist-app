using System;
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
            var service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";

            // Not recommended to use flag AcceptInsecureCertificates. However, I decided to switch on the flag to save time.
            var options = new FirefoxOptions() {  AcceptInsecureCertificates = true };

            Driver = new FirefoxDriver(service, options);
            Driver.Navigate().GoToUrl(LoginUrl);
            Driver.WaitUntilPageIsReady();
            
            AdditionalSetup();
        }

        protected void AuthenticateWithDefaultUser()
        {
            FillInput("UserName","test");
            FillInput("Password","pwd123");
            ClickOnButton("login");
            
            Driver.WaitUntilPageIsReady();
        }

        protected void FillInput(string id, string text)
        {
            Driver.FindElement(By.Id(id)).SendKeys($"{text}");
        }

        protected void ClickOnButton(string id)
        {
            Driver.FindElement(By.Id(id)).Submit();            
        }
        protected void ClickOnLink(string id)
        {
            Driver.FindElement(By.Id(id)).Click();           
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