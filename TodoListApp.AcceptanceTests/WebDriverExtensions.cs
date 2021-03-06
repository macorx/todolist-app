﻿using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TodoListApp.AcceptanceTests
{
    public static class WebDriverExtensions
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(2);
        
        
        public static void TakeScreenshot(this IWebDriver driver, string testName)
        {
            var tmpPath = Path.Combine(Path.GetTempPath(), "Screenshots");
            if (!Directory.Exists(tmpPath))
                Directory.CreateDirectory(tmpPath);
                    
            var fullPath = Path.Combine(tmpPath, $"{testName}_{DateTime.Now:ddMMyyyyHHmm}.png");
            
            var ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(fullPath);
        }        
        
        public static void WaitUntilPageIsReady(this IWebDriver driver)
        {
            new WebDriverWait(driver, DefaultTimeout).Until(
                d => ((IJavaScriptExecutor) d).ExecuteScript("return document.readyState").Equals("complete"));            
        }

        public static IWebElement WaitUntilVisible(this IWebDriver driver, By by)
        {
            IWebElement expectedElement = default;
            new WebDriverWait(driver, DefaultTimeout).Until(condition =>
            {
                try
                {
                    expectedElement = driver.FindElement(by);
                    return expectedElement.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
            return expectedElement;
        }
        
        public static bool WaitUntilInvisible(this IWebDriver driver, By by)
        {
            return new WebDriverWait(driver, DefaultTimeout).Until(condition =>
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(by);
                    return !elementToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
                catch (NoSuchElementException)
                {
                    return true;
                }
            });            
        }
    }
}