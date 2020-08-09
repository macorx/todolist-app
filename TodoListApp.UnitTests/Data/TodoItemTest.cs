using System;
using NUnit.Framework;
using TodoListApp.WebApp.Data;

namespace TodoListApp.UnitTests.Data
{
    public class TodoItemTest
    {
        [Test]
        public void CreateItemWithDefaultValues()
        {
            var userId = "e261aeb6-58c8-4057-bc64-7235e5ebd1fc";
            var description = "task description";
            var item = new TodoItem(userId, description);

            Assert.That(item.Description, Is.EqualTo(description));
            Assert.That(item.UserId, Is.EqualTo(userId));
            Assert.IsFalse(item.IsComplete);
            Assert.That(item.DateOfCreation, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(1)));
            Assert.IsNull(item.DateOfLastUpdate);
        }

        [Test]
        public void ItemsHaveDifferentIds()
        {
            var item = new TodoItem(string.Empty, string.Empty);
            var anotherItem = new TodoItem(string.Empty, string.Empty); 
            Assert.That(item.Id, Is.Not.EqualTo(anotherItem.Id));
        }
    }
}