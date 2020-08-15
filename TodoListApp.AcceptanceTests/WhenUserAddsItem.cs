using System;
using NUnit.Framework;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserAddsItem : AcceptanceTestBase
    {
        private TodoListPage todoListPage;

        protected override void AdditionalSetup()
        {
            todoListPage = SignIn();
        }
        
        [Test]
        public void NewItemIsDisplayedOnTodoList()
        {
            var creationTime = DateTime.Now;
            var description = "Item " + creationTime.ToString("s");
            todoListPage.AddItem()
                .WithDescription(description)
                .Submit();

            var todoItem = todoListPage.GetItem(description);
            Assert.IsNotNull(todoItem);
            Assert.That(todoItem.State, Is.EqualTo("Pending"));            
            Assert.That(todoItem.DateOfCreation, Is.EqualTo(creationTime).Within(TimeSpan.FromSeconds(10)));
            Assert.IsNull(todoItem.DateOfLastUpdate);
        }
    }
}