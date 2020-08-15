using System.Linq;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public class TodoListPage : ISignedPage
    {
        public IWebDriver Driver { get; }

        public TodoListPage(IWebDriver driver)
        {
            this.Driver = driver;
        }

        public AddPage AddItem()
        {
            Driver.FindElement(By.Id("add")).Click();
            return new AddPage(Driver);
        }

        public TodoItem GetItem(string description)
        {
            var itemIndex = 1;
            foreach (var todoItem in Driver
                .FindElements(By.CssSelector("table tbody tr"))
                .Select(item => new TodoItem(itemIndex, item.FindElements(By.CssSelector("td")))))
            {
                if (todoItem.Description.Equals(description))
                    return todoItem;
                itemIndex++;
            }
            return default;
        }

        public void DeleteItem(string description)
        {
            var todoItem = GetItem(description);
            
            Driver
                .FindElement(By.CssSelector($"#grid table tbody tr:nth-of-type({todoItem.Index}) button[data-target='delete']"))
                .Click();
            
            ConfirmOperation();
        }

        private void ConfirmOperation()
        {
            Driver.WaitUntilVisible("confirm");
            Driver.FindElement(By.Id("confirm")).Click();
        }

        public int CountItems()
        {
            return Driver.FindElements(By.CssSelector("table tbody tr")).Count;            
        }
    }
}