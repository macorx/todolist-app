using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoListApp.WebApp.Controllers;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.UnitTests.Controllers
{
    public class WhenEditingItem : ControllerTestBase
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
        
        [TestCase(true)]
        [TestCase(false)]
        public void ReturnsViewWithItem(bool isDone)
        {
            var id = "0bea3da1-5e04-40c3-b949-22f7c25a64bb";
            var description = "My task";
            
            var todoItem = new TodoItemBuilder()
                .WithId(id)
                .WithDescription(description)
                .WithIsDone(isDone)
                .Build();

            todoItemsRepository.Setup(t => t.GetAllForUser(currentUser))
                .Returns(new[] {todoItem}.AsQueryable());

            var viewResult = controller.Edit(id).As<ViewResult>();
            var viewModel = viewResult.Model.As<EditItemViewModel>();
            
            Assert.That(viewModel.Id, Is.EqualTo(id));
            Assert.That(viewModel.Description, Is.EqualTo(description));
            Assert.That(viewModel.IsDone, Is.EqualTo(isDone));
        }

        [Test]
        public void ReturnsNotFoundWhenItemDoesNotExist()
        {
            todoItemsRepository.Setup(t => t.GetAllForUser(It.IsAny<string>()))
                .Returns(Enumerable.Empty<TodoItem>().AsQueryable());

            Assert.IsInstanceOf<NotFoundResult>(controller.Edit("invalid-id"));
        }
        
        
        [Test]
        public async Task ReturnsNotFoundWhenItemBeingSavedDoesNotExist()
        {
            var id = "0bea3da1-5e04-40c3-b949-22f7c25a6411bddb";            
            var viewModel = new EditItemViewModel() { Id = id, Description = "description" };            
            
            todoItemsRepository.Setup(t => t.GetAllForUser(It.IsAny<string>()))
                .Returns(Enumerable.Empty<TodoItem>().AsQueryable());

            Assert.IsInstanceOf<NotFoundResult>(await controller.Edit(viewModel));
        }

        [Test]
        public async Task AndModelIsInvalidReturnsToViewWithErrors()
        {
            controller.ModelState.AddModelError("Description","Description is required");

            var id = "0bea3da1-5e04-40c3-b949-22f7c25a6411bddb";
            var viewModel = new EditItemViewModel {Id = id};
            
            var result = await controller.Edit(viewModel);

            var viewResult = result.As<ViewResult>();
            var returnedViewModel = viewResult.Model.As<EditItemViewModel>();
            
            Assert.AreSame(viewModel, returnedViewModel);
        }

        [Test]
        public void SaveChangesInTheDatabase()
        {
            var id = "0bea3da1-5e04-40c3-b949-22f7c25a6411bddb";
            var viewModel = new EditItemViewModel
            {
                Id = id,
                Description = "new description",
                IsDone = true
            };
            
            var todoItem = new TodoItemBuilder()
                .WithId(id)
                .WithDescription("old description")
                .WithIsDone(false)
                .Build();
            
            todoItemsRepository.Setup(r => r.GetAllForUser(currentUser))
                .Returns(new[] { todoItem }.AsQueryable());
            
            controller.Edit(viewModel);
            
            Assert.That(todoItem.IsDone, Is.EqualTo(viewModel.IsDone));
            Assert.That(todoItem.Description, Is.EqualTo(viewModel.Description));
            Assert.That(todoItem.DateOfLastUpdate, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(2)));
            
            todoItemsRepository.Verify(t => t.ApplyChanges());
        }

        [Test]
        public async Task ReturnsToIndexAfterItemIsEdited()
        {
            var id = "0bea3da1-5e04-40c3-b949-22f7c25a6411bddb";            
            var viewModel = new EditItemViewModel() { Id = id, Description = "description" };
            
            todoItemsRepository.Setup(r => r.GetAllForUser(currentUser))
                .Returns(new[] { new TodoItemBuilder().WithId(id).Build() }.AsQueryable());            

            var result = await controller.Edit(viewModel);

            var redirectToAction = result.As<RedirectToActionResult>();
            Assert.That(redirectToAction.ActionName, Is.EqualTo("Index"));
        }
    }
}