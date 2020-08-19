using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Data;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITodoItemRepository todoItemRepository;

        public AdminController(UserManager<IdentityUser> userManager, ITodoItemRepository todoItemRepository)
        {
            this.userManager = userManager;
            this.todoItemRepository = todoItemRepository;
        }

        public IActionResult Index()
        {
            var adminViewModel = new AdminViewModel
            {
                TotalUsers = userManager.Users.Count(),
                TotalPendingTasks = todoItemRepository.GetAll().Count(t => !t.IsDone),
                TotalCompletedTasks = todoItemRepository.GetAll().Count(t => t.IsDone),
            };
            
            return View(adminViewModel);
        }
        
    }
}