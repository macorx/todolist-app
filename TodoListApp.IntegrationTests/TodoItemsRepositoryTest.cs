using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
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
        public async Task AddTodoItemForUser()
        {
            var description = "Send meeting notes to John";
            var isDone = true;
            var dateOfCreation = DateTime.Now;
            var dateOfLastUpdate = DateTime.Now.AddMinutes(10);

            var todoItem = new TodoItem(userId, description);

            todoItem.SetProperty(nameof(TodoItem.DateOfCreation), dateOfCreation);
            todoItem.SetProperty(nameof(TodoItem.DateOfLastUpdate), dateOfLastUpdate);
            todoItem.SetProperty(nameof(TodoItem.IsDone), isDone);
            await repository.Add(todoItem);
            
            var items = repository.GetAllForUser(userId);
            
            Assert.That(items.Count(), Is.EqualTo(1));

            var savedTodoItem = items.First();
            Assert.That(savedTodoItem.Description, Is.EqualTo(description));
            Assert.That(savedTodoItem.IsDone, Is.EqualTo(isDone));
            Assert.That(savedTodoItem.DateOfCreation, Is.EqualTo(dateOfCreation));
            Assert.That(savedTodoItem.DateOfLastUpdate, Is.EqualTo(dateOfLastUpdate));
        }

        [Test]
        public async Task ReturnsOnlyItemsFromUser()
        {
            await repository.Add(new TodoItem(userId, "userOne description"));
            await repository.Add(new TodoItem(otherUserId, "userTwo description"));

            var items = repository.GetAllForUser(otherUserId).ToList();
            
            Assert.That(items.Count, Is.EqualTo(1));
            Assert.That(items[0].Description, Is.EqualTo("userTwo description"));
        }

        [Test]
        public async Task ReturnsAllItemsFromAllUsers()
        {
            await repository.Add(new TodoItem(userId, "userOne description"));
            await repository.Add(new TodoItem(otherUserId, "userTwo description"));

            var items = repository.GetAll().ToList();
            
            Assert.That(items.Count, Is.EqualTo(2));
            Assert.That(items[0].Description, Is.EqualTo("userOne description"));
            Assert.That(items[1].Description, Is.EqualTo("userTwo description"));
        }

        [Test]
        public async Task DeletesTodoItem()
        {
            await repository.Add(new TodoItem(userId, "userOne description"));

            var storedItem = repository.GetAllForUser(userId).First();

            await repository.Delete(storedItem);
            
            Assert.That(repository.GetAllForUser(userId).Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ApplyChanges()
        {
            await repository.Add(new TodoItem(userId, "userOne description"));

            var item = repository.GetAllForUser(userId).First();
            item.IsDone = true;
            
            await repository.ApplyChanges();

            using var newScope = Factory.Services.CreateScope();
            
            var newRepository = newScope.ServiceProvider.GetService<ITodoItemRepository>();
            
            var loadedItem = newRepository.GetAllForUser(userId).First();
            Assert.That(loadedItem.IsDone, Is.EqualTo(item.IsDone));
        }
    }
}