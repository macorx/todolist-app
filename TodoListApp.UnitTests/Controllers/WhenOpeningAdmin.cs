using System.Collections.Generic;
using System.Linq;
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
        public async Task ReturnsViewModelWithTotals()
        {
            const int totalUsers = 5;
            var users = GivenExistingUsers(totalUsers);

            AssumeExistingTodoItems(
                GivenUserHasTodoItems(users[0].Id, done: 2, pending: 1),
                GivenUserHasTodoItems(users[1].Id, done: 4, pending: 3));

            var result = await controller.Index();
            
            var viewModel = result.As<ViewResult>().Model.As<AdminViewModel>();
            Assert.That(viewModel.TotalUsers, Is.EqualTo(totalUsers));
            Assert.That(viewModel.TotalDoneItems, Is.EqualTo(6));
            Assert.That(viewModel.TotalPendingItems, Is.EqualTo(4));
        }

        [Test]
        public async Task ReturnsViewModelWithUserDetails()
        {
            var userOne = new IdentityUser("test");
            var userTwo = new IdentityUser("test1");
            var userThree = new IdentityUser("test2");
            AssumeExistingUsers(userOne, userTwo, userThree);            
            
            AssumeExistingTodoItems(
                GivenUserHasTodoItems(userOne.Id, done: 1, pending: 2),
                GivenUserHasTodoItems(userTwo.Id, done: 3, pending: 4));

            var result = await controller.Index();
            
            var adminViewModel = result.As<ViewResult>().Model.As<AdminViewModel>();
            AssertUserDetails(adminViewModel, userOne, 1, 2);
            AssertUserDetails(adminViewModel, userTwo, 3, 4);
            AssertUserDetails(adminViewModel, userThree, 0, 0);
        }

        [Test]
        public async Task GetsOnlyUsersWithRoleUser()
        {
            AssumeExistingUsers(new IdentityUser());                
            
            await controller.Index();
            
            userManager.Verify(u => u.GetUsersInRoleAsync("User"), Times.Once);
        }

        private static void AssertUserDetails(AdminViewModel viewModel, IdentityUser userOne, int doneItems, int pendingItems)
        {
            var userDetails = viewModel.Users.FirstOrDefault(u => u.UserName == userOne.UserName);
            Assert.IsNotNull(userDetails);
            Assert.That(userDetails.TotalDoneItems, Is.EqualTo(doneItems));
            Assert.That(userDetails.TotalPendingItems, Is.EqualTo(pendingItems));
        }

        private void AssumeExistingTodoItems(params IEnumerable<TodoItem>[] todoItems)
        {
            var all = todoItems.SelectMany(list => list).ToArray();
            todoItemRepository.Setup(r => r.GetAll()).Returns(all.AsQueryable());
        }

        private IEnumerable<TodoItem> GivenUserHasTodoItems(string userId, int done, int pending)
        {
            return Enumerable.Range(0, done).Select(i => new TodoItem(userId, string.Empty) { IsDone = true })
                .Concat(Enumerable.Range(0, pending).Select(i => new TodoItem(userId, string.Empty) { IsDone = false }));
        }

        private IdentityUser[] GivenExistingUsers(int totalUsers)
        {
            var users = Enumerable
                .Range(0, totalUsers)
                .Select(i => new IdentityUser($"user{i}"))
                .ToArray();
            
            AssumeExistingUsers(users);
            return users;
        }

        private void AssumeExistingUsers(params IdentityUser[] users)
        {
            userManager.Setup(u => u.GetUsersInRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(users.ToList());
        }
    }
}