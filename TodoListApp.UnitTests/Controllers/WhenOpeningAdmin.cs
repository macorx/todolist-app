using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoListApp.WebApp.Controllers;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.UnitTests.Controllers
{
    public class WhenOpeningAdmin
    {
        private AdminController controller;
        private Mock<UserManager<IdentityUser>> userManager;
        private Mock<ITodoItemRepository> todoItemRepository;

        [SetUp]
        public void SetUp()
        {
            userManager = MockHelpers.ForUserManager();
            todoItemRepository = new Mock<ITodoItemRepository>();
            
            controller = new AdminController(userManager.Object, todoItemRepository.Object);
        }

        [Test]
        public void OnlyAdminCanAccessController()
        {
            var attribute = typeof(AdminController).GetCustomAttribute<AuthorizeAttribute>();
            Assert.IsNotNull(attribute);
            Assert.That(attribute.Roles, Is.EqualTo("Admin"));
        }
        
        [Test]
        public void ReturnsViewModelWithTotals()
        {
            const int totalUsers = 5;
            AssumeExistingUsersCount(totalUsers);

            var todoList = new List<TodoItem>();
            todoList.AddRange(GivenUserHasTodoItems("userId", count: 4, done: true));
            todoList.AddRange(GivenUserHasTodoItems("userId", count: 3, done: false));
            todoList.AddRange(GivenUserHasTodoItems("userId2", count: 2, done: true));
            todoList.AddRange(GivenUserHasTodoItems("userId2", count: 1, done: false));
            todoItemRepository.Setup(r => r.GetAll()).Returns(todoList.AsQueryable());

            var result = controller.Index();
            
            var viewModel = result.As<ViewResult>().Model.As<AdminViewModel>();
            Assert.That(viewModel.TotalUsers, Is.EqualTo(totalUsers));
            Assert.That(viewModel.TotalPendingTasks, Is.EqualTo(4));
            Assert.That(viewModel.TotalCompletedTasks, Is.EqualTo(6));
        }

        private IEnumerable<TodoItem> GivenUserHasTodoItems(string userId, int count, bool done)
        {
            return Enumerable.Range(0, count)
                .Select(i => new TodoItem(userId, string.Empty) { IsDone = done });
        }

        private void AssumeExistingUsersCount(int totalUsers)
        {
            userManager.Setup(u => u.Users)
                .Returns(new IdentityUser[totalUsers].AsQueryable());
        }
    }
}