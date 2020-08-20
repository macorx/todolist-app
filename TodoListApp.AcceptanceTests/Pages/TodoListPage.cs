﻿using System.Linq;
using System.Threading;
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
        
        public void DeleteItemWithDescription(string description)
        {
            var todoItem = GetItem(description);

            Driver
                .FindElement(By.CssSelector($"#grid table tbody tr:nth-of-type({todoItem.Index}) button[data-target='delete']"))
                .Click();
            
            Confirm();
        }        
        
        public EditPage EditItemWithDescription(string description)
        {
            var todoItem = GetItem(description);
            
            Driver
                .FindElement(By.CssSelector($"#grid table tbody tr:nth-of-type({todoItem.Index}) a[data-target='edit']"))
                .Click();
            
            return new EditPage(Driver);
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

        public TodoListPage DeleteAllItems()
        {
            foreach (var row in Driver.FindElements(By.CssSelector("table tbody tr")))
            {
                var description = row.FindElements(By.CssSelector("td"))[1].Text;
                DeleteItemWithDescription(description);
            }

            return this;
        }

        private void Confirm()
        {
            Driver.WaitUntilVisible(By.Id("deleteModal"));

            // Modal is visible, however Bootstrap's modal state may not be ready to react to button click.
            Thread.Sleep(500);

            Driver.FindElement(By.Id("confirm")).Click();
            
            Driver.WaitUntilInvisible(By.CssSelector("body > div.modal-backdrop"));
        }

        public int CountItems()
        {
            return Driver.FindElements(By.CssSelector("table tbody tr")).Count;            
        }
    }
}