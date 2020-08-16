using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoListApp.WebApp.Controllers;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.UnitTests.Controllers
{
    public class WhenGettingListOfTodoItems : ControllerTestBase
    {
        private TodoItemsController controller;
        private Mock<ITodoItemRepository> todoItemsRepository;
        private string currentUser;

        [SetUp]
        public void SetUp()
        {
            todoItemsRepository = new Mock<ITodoItemRepository>();
            controller = new TodoItemsController(todoItemsRepository.Object);
            
            currentUser = "e261aeb6-58c8-4057-bc64-7235e5ebd1fc";
            AssumeUserIdIsSignedIn(controller, currentUser);
        }

        [Test]
        public void ReturnsViewResult()
        {
            Assert.IsInstanceOf<ViewResult>(controller.Index());
        }
        
        [Test]
        public void ReturnsTodoItemsFromUser()
        {
            var todoItems = GivenTodoItemsForUsers(currentUser);
            AssumeUserHasTodoItems(currentUser, todoItems);

            var viewResult = controller.IndexGrid().As<PartialViewResult>();
            var viewModelItems = viewResult.Model.As<IQueryable<TodoItem>>().ToArray();
            Assert.That(viewModelItems.Count, Is.EqualTo(todoItems.Count));
            AssertTodoItem(viewModelItems[0], todoItems[0]);
            AssertTodoItem(viewModelItems[1], todoItems[1]);
        }
        
        private static List<TodoItem> GivenTodoItemsForUsers(string userId)
        {
            return new List<TodoItem>()
            {
                new TodoItem(userId, "first item"),
                new TodoItem(userId, "second item!")
            };
        }
        
        private void AssumeUserHasTodoItems(string userId, List<TodoItem> todoItems)
        {
            todoItemsRepository.Setup(t => t.GetAll(userId))
                .Returns(todoItems.AsQueryable());
        }        

        private void AssertTodoItem(TodoItem viewModelItem, TodoItem todoItem)
        {
            Assert.That(viewModelItem.Id, Is.EqualTo(todoItem.Id));
            Assert.That(viewModelItem.Description, Is.EqualTo(todoItem.Description));
            Assert.That(viewModelItem.IsDone, Is.EqualTo(todoItem.IsDone));
            Assert.That(viewModelItem.DateOfCreation, Is.EqualTo(todoItem.DateOfCreation));
            Assert.That(viewModelItem.DateOfLastUpdate, Is.EqualTo(todoItem.DateOfLastUpdate));
        }
    }
}