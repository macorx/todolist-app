using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
            return View(repository.GetAll(CurrentUserId));
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

    }
}