using System;
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
            const string description = "Review notes from last meeting.";
            AddItemWithDescription(description);

            var items = GetItems();
            Assert.That(items.Length, Is.EqualTo(1));
            var todoItem = items.First();

            Assert.That(todoItem.State, Is.EqualTo("Pending"));            
            Assert.That(todoItem.Description, Is.EqualTo(description));            
            Assert.That(todoItem.DateOfCreation, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(10)));
            Assert.IsNull(todoItem.DateOfLastUpdate);
        }

        private TodoItem[] GetItems()
        {
            var items = Driver.FindElements(By.CssSelector("table tbody tr"));
            return items.Select(item => new TodoItem(items[0].FindElements(By.CssSelector("td")))).ToArray();
        }

        [Test]
        public void UserCanDeleteExistingItem()
        {
            AddItemWithDescription("Delete me");
            
            ClickOnButton("delete");
            Assert.That(CountItems(), Is.EqualTo(0));
        }
        
        private int CountItems()
        {
            return Driver.FindElements(By.CssSelector("table tbody tr")).Count;            
        }
        
        private void AddItemWithDescription(string description)
        {
            ClickOnLink("add");
            
            FillInput("Description",description);
            ClickOnButton("add");
        }
    }
}