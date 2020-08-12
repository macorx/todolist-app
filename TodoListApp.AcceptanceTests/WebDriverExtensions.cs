using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TodoListApp.AcceptanceTests
{
    public static class WebDriverExtensions
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(2);
        
        public static void WaitUntilPageIsReady(this IWebDriver driver)
        {
            new WebDriverWait(driver, DefaultTimeout).Until(
                d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));            
        }

        public static void WaitUntilVisible(this IWebDriver driver, string elementId)
        {
            new WebDriverWait(driver, DefaultTimeout).Until(
                d => d.FindElement(By.Id(elementId)) != null);
        }
    }
}