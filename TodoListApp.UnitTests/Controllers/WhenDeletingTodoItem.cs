using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoListApp.WebApp.Controllers;
using TodoListApp.WebApp.Data;

namespace TodoListApp.UnitTests.Controllers
{
    public class WhenDeletingTodoItem : ControllerTestBase
    {
        private TodoItemsController controller;
        private Mock<ITodoItemRepository> todoItemsRepository;
        private const string CurrentUser = "e261aeb6-58c8-4057-bc64-7235e5ebd1fc";
        private const string ExistingTodoItemId = "75153d9d-5548-4de2-8c06-7175220d3d93";

        [SetUp]
        public void SetUp()
        {
            todoItemsRepository = new Mock<ITodoItemRepository>();
            controller = new TodoItemsController(todoItemsRepository.Object);
            
            AssumeUserIdIsSignedIn(controller, CurrentUser);
        }

        [Test]
        public async Task DeletesItemFromRepository()
        {
            AssumeExistingTodoItems(CreateTodoItem(CurrentUser, ExistingTodoItemId));

            await controller.Delete(ExistingTodoItemId);
            
            todoItemsRepository.Verify(r => 
                r.Delete(It.Is<TodoItem>(t => t.Id == ExistingTodoItemId)));
        }
        
        private void AssumeExistingTodoItems(params TodoItem[] todoItems)
        {
            todoItemsRepository.Setup(r => r.GetAll(CurrentUser))
                .Returns(todoItems.AsQueryable());
        }

        private TodoItem CreateTodoItem(string user, string id)
        {
            var todoItem = (TodoItem)Activator.CreateInstance(typeof(TodoItem), true);
            todoItem.SetProperty("Id", id);
            todoItem.SetProperty("UserId", user);
            return todoItem;
        }
        

        [Test]
        public async Task AndItemsDoesNotExistReturnsNoContent()
        {
            var invalidTodoItemId = "234dfd-58c8-4057-bc64-7235e5ebd1fc";
            var result = await controller.Delete(invalidTodoItemId);
            
            Assert.IsInstanceOf<NoContentResult>(result);            
            todoItemsRepository.Verify(r => r.Delete(It.IsAny<TodoItem>()), Times.Never());
        }

        [Test]
        public async Task ReturnsNoContent()
        {
            AssumeExistingTodoItems(CreateTodoItem(CurrentUser, ExistingTodoItemId));            
            
            Assert.IsInstanceOf<NoContentResult>(await controller.Delete(ExistingTodoItemId));
        }

        
    }
}