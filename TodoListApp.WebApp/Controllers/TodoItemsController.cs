using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    public class TodoItemsController : Controller
    {
        private readonly ITodoItemRepository repository;

        private string CurrentUserId => HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public TodoItemsController(ITodoItemRepository repository)
        {
            this.repository = repository;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }          

        [HttpGet]
        public IActionResult IndexGrid()
        {
            return PartialView("_IndexGrid", repository.GetAllForUser(CurrentUserId));
        }

        [HttpGet] 
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewTodoItemViewModel newTodoItem)
        {
            if (!ModelState.IsValid)
                return View(newTodoItem);
            
            await repository.Add(new TodoItem(CurrentUserId, newTodoItem.Description));
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var todoItem = repository.GetAllForUser(CurrentUserId).FirstOrDefault(t => t.Id == id);
            if (todoItem != null)
                await repository.Delete(todoItem);
            
            return new NoContentResult();
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var item = repository.GetAllForUser(CurrentUserId).FirstOrDefault(t => t.Id == id);
            if (item == null)
                return new NotFoundResult();
            
            var viewModel = new EditItemViewModel(item);
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            
            var item = repository.GetAllForUser(CurrentUserId).FirstOrDefault(t => t.Id == viewModel.Id);
            
            if (item == null)
                return new NotFoundResult();

            viewModel.Update(item);

            await repository.ApplyChanges();

            return RedirectToAction("Index");
        }
    }
}