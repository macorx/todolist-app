using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TodoListApp.UnitTests;
using TodoListApp.WebApp.Data;

namespace TodoListApp.IntegrationTests
{
    public class TodoItemsRepositoryTest : IntegrationTestBase
    {
        private string userId;
        private string otherUserId;
        
        private ITodoItemRepository repository;

        protected override void AdditionalSetup()
        {
            var userManager = ServiceProvider.GetService<UserManager<IdentityUser>>();
            var users = userManager.Users.ToArray();
            userId = users[0].Id;
            otherUserId = users[1].Id;
            
            repository = ServiceProvider.GetService<ITodoItemRepository>();            
        }

        [Test]
        public void AddTodoItemForUser()
        {
            var description = "Send meeting notes to John";
            var isComplete = true;
            var dateOfCreation = DateTime.Now;
            var dateOfLastUpdate = DateTime.Now.AddMinutes(10);

            var todoItem = new TodoItem(userId, description);

            todoItem.SetProperty(nameof(TodoItem.DateOfCreation), dateOfCreation);
            todoItem.SetProperty(nameof(TodoItem.DateOfLastUpdate), dateOfLastUpdate);
            todoItem.SetProperty(nameof(TodoItem.IsComplete), isComplete);
            repository.Add(todoItem);
            
            var items = repository.GetAll(userId);
            
            Assert.That(items.Count(), Is.EqualTo(1));

            var savedTodoItem = items.First();
            Assert.That(savedTodoItem.Description, Is.EqualTo(description));
            Assert.That(savedTodoItem.IsComplete, Is.EqualTo(isComplete));
            Assert.That(savedTodoItem.DateOfCreation, Is.EqualTo(dateOfCreation));
            Assert.That(savedTodoItem.DateOfLastUpdate, Is.EqualTo(dateOfLastUpdate));
        }

        [Test]
        public void ReturnsOnlyItemsFromUser()
        {
            repository.Add(new TodoItem(userId, "userOne description"));
            repository.Add(new TodoItem(otherUserId, "userTwo description"));

            var items = repository.GetAll(otherUserId).ToList();
            
            Assert.That(items.Count, Is.EqualTo(1));
            Assert.That(items[0].Description, Is.EqualTo("userTwo description"));
        }
    }
}