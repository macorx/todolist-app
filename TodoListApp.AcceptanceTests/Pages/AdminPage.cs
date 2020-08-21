using System.Collections.Generic;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public class AdminPage : ISignedPage
    {
        public IWebDriver Driver { get; }

        public int TotalUsers => GetValueFromElement(By.Id("totalUsers"));
        public int TotalPendingItems => GetValueFromElement(By.Id("totalPendingItems"));
        public int TotalDoneItems => GetValueFromElement(By.Id("totalDoneItems"));

        public AdminPage(IWebDriver driver)
        {
            this.Driver = driver;
        }

        private int GetValueFromElement(By by)
        {
            return int.Parse(Driver.FindElement(by).Text);
        }

        public IEnumerable<(string user, int pending, int done)> GetItemsPerUser()
        {
            var webElements = Driver.FindElements(By.CssSelector("ul#userDetails li"));
            foreach (var webElement in webElements)
            {
                var user = webElement.FindElement(By.CssSelector("span.user")).Text;
                var pending = int.Parse(webElement.FindElement(By.CssSelector("span.pending")).Text);
                var done = int.Parse(webElement.FindElement(By.CssSelector("span.done")).Text);
                yield return (user, pending, done);
            }
        }
    }
}