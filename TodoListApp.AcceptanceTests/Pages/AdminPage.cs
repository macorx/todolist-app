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
    }
}