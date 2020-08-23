using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public class AddPage : ISignedPage
    {
        public IWebDriver Driver { get; }
        
        public AddPage(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public AddPage WithDescription(string description)
        {
            Driver.FindElement(By.Id("Description")).SendKeys(description);
            return this;
        }

        public void Submit()
        {
            Driver.FindElement(By.Id("add")).Submit();
            Driver.WaitUntilPageIsReady();
        }

    }
}