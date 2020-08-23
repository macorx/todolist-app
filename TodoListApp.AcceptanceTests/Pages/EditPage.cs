using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public class EditPage : ISignedPage
    {
        public IWebDriver Driver { get; }

        public EditPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public EditPage MarkAsDone()
        {
            Driver.FindElement(By.Id("IsDone")).Click();
            return this;
        }

        public EditPage Description(string description)
        {
            var input = Driver.FindElement(By.Id("Description"));
            input.Clear();
            input.SendKeys(description);
            
            return this;
        }

        public TodoListPage Submit()
        {
            Driver.FindElement(By.Id("save")).Submit();
            Driver.WaitUntilPageIsReady();
            return new TodoListPage(Driver);
        }
    }
}