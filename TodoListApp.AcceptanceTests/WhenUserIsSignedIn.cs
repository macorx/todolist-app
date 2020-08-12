using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserIsSignedIn : AcceptanceTestBase
    {
        protected override void AdditionalSetup()
        {
            AuthenticateWithDefaultUser();
        }
        
        [Test]
        public void UserCanAddNewItem()
        {
            var creationTime = DateTime.Now;
            var description = "Test item from " + creationTime.ToString("s");
            AddItemWithDescription(description);

            var todoItem = GetItemWithDescription(description);
            Assert.IsNotNull(todoItem);
            Assert.That(todoItem.State, Is.EqualTo("Pending"));            
            Assert.That(todoItem.DateOfCreation, Is.EqualTo(creationTime).Within(TimeSpan.FromSeconds(10)));
            Assert.IsNull(todoItem.DateOfLastUpdate);
        }

        private TodoItem GetItemWithDescription(string description)
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

        [Test]
        public void UserCanDeleteExistingItem()
        {
            var currentCount = CountItems();
            var description = "Test item from " + DateTime.Now.ToString("s");            
            AddItemWithDescription(description);
            
            var todoItem = GetItemWithDescription(description);

            GetDeleteButtonFor(todoItem).Click();
            ConfirmOperation();
            
            Assert.That(CountItems(), Is.EqualTo(currentCount));
        }

        private IWebElement GetDeleteButtonFor(TodoItem todoItem)
        {
            return Driver
                .FindElement(By.CssSelector($"#grid table tbody tr:nth-of-type({todoItem.Index}) button[data-target='delete']"));
        }

        private int CountItems()
        {
            return Driver.FindElements(By.CssSelector("table tbody tr")).Count;            
        }
        
        private void AddItemWithDescription(string description)
        {
            ClickOnLink("add");
            
            FillInput("Description",description);
            ClickOnButtonWithId("add");
        }
    }
}