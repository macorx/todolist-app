using System.Collections.Generic;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public class AdminPage : ISignedPage
    {
        public IWebDriver Driver { get; }

        public int TotalUsers => GetValueFromElement(By.Id("totalUsers"));
        public int TotalPendingTasks => GetValueFromElement(By.Id("totalPendingTasks"));
        public int TotalCompletedTasks => GetValueFromElement(By.Id("totalCompletedTasks"));

        public AdminPage(IWebDriver driver)
        {
            this.Driver = driver;
        }

        private int GetValueFromElement(By by)
        {
            return int.Parse(Driver.FindElement(by).Text);
        }

        public IEnumerable<(string user, int pending, int completed)> GetItemsPerUser()
        {
            var webElements = Driver.FindElements(By.CssSelector("ul li"));
            foreach (var webElement in webElements)
            {
                var user = webElement.FindElement(By.CssSelector("span.user")).Text;
                var pending = int.Parse(webElement.FindElement(By.CssSelector("span.pending")).Text);
                var completed = int.Parse(webElement.FindElement(By.CssSelector("span.completed")).Text);
                yield return (user, pending, completed);
            }
        }
    }
}