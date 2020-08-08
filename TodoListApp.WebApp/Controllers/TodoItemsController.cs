using Microsoft.AspNetCore.Mvc;

namespace TodoListApp.WebApp.Controllers
{
    public class TodoItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}