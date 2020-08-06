using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserSignsIn
    {
        private IWebDriver driver;

        private const string BasePath = "https://localhost:5001";

        [SetUp]
        public void Setup()
        {
            driver = new FirefoxDriver(new FirefoxOptions() { AcceptInsecureCertificates = true });
            driver.Navigate().GoToUrl($"{BasePath}/Identity/Account/Login");
            driver.WaitUntilPageIsReady();            
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test]
        public void DisplaysUsersTodoList()
        {
            driver.FindElement(By.CssSelector("input#UserName")).SendKeys("test");
            driver.FindElement(By.CssSelector("input#Password")).SendKeys("pwd123");
            driver.FindElement(By.CssSelector("button")).Submit();
            
            driver.WaitUntilPageIsReady();
            
            Assert.That(driver.Url, Is.EqualTo($"{BasePath}/"));
        }
    }
}