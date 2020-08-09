using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoListApp.WebApp.Controllers;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.UnitTests.Controllers
{
    public class WhenAddingNewTodoItem : ControllerTestBase
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
        public void ReturnsViewToAddItem()
        {
            Assert.IsInstanceOf<ViewResult>(controller.Add());
        }
        
        [Test]
        public void SavesItInTheDatabase()
        {
            var description = "Call Mark";
            var newTodoItem = new NewTodoItemViewModel() { Description = description };

            controller.Add(newTodoItem);
            
            todoItemsRepository.Verify(r => r.Add(
                It.Is<TodoItem>(t => 
                    t.Description == description && t.UserId == currentUser)));
        }

        [Test]
        public void ReturnsToIndexAfterItemIsAdded()
        {
            var newTodoItem = new NewTodoItemViewModel() { Description = "description" };

            var result = controller.Add(newTodoItem);

            var redirectToAction = result.As<RedirectToActionResult>();
            Assert.That(redirectToAction.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void AndModelIsInvalidReturnsToViewWithErrors()
        {
            controller.ModelState.AddModelError("Description","Description is required");

            var viewModel = new NewTodoItemViewModel();
            var result = controller.Add(viewModel);

            var viewResult = result.As<ViewResult>();
            var returnedViewModel = viewResult.Model.As<NewTodoItemViewModel>();
            
            Assert.AreSame(viewModel, returnedViewModel);
        }

    }
}