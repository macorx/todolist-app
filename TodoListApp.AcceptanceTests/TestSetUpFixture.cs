using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TodoListApp.AcceptanceTests
{
    public static class WebDriver
    {
        public static IWebDriver Instance { get; set; }
    }
    
    
    [SetUpFixture]
    public class TestSetUpFixture
    {

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var service = FirefoxDriverService.CreateDefaultService();
            service.Host = "::1";

            // Not recommended to use flag AcceptInsecureCertificates. However, I decided to switch on the flag to save time.
            var options = new FirefoxOptions() {  AcceptInsecureCertificates = true };
            options.AddArguments("--headless");            

            WebDriver.Instance = new FirefoxDriver(service, options);
            WebDriver.Instance.WaitUntilPageIsReady();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            WebDriver.Instance.Quit();
            WebDriver.Instance.Dispose();            
        }
     }
}