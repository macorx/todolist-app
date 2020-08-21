using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index()
        {
            var users = (await userManager.GetUsersInRoleAsync("User"))
                .Select(user => new {user.Id, user.UserName})
                .ToArray();

            var todoItems = todoItemRepository.GetAll()
                .GroupBy(item => item.UserId)
                .Select(t => new
                {
                    UserId = t.Key,
                    Done = t.Count(i => i.IsDone),
                    Pending = t.Count(i => !i.IsDone)
                })
                .ToArray();
            
            var userDetails = from user in users
                join item in todoItems on user.Id equals item.UserId into gj
                from todoItem in gj.DefaultIfEmpty(new { UserId = string.Empty, Done = 0, Pending = 0 })
                select new UserDetailsViewModel {UserName = user.UserName, TotalDoneItems=todoItem.Done, TotalPendingItems=todoItem.Pending};
            
            var adminViewModel = new AdminViewModel
            {
                TotalUsers = users.Count(),
                TotalPendingItems = todoItems.Sum(item => item.Pending),
                TotalDoneItems = todoItems.Sum(item => item.Done),
                Users = userDetails.ToArray()
            };
            
            return View(adminViewModel);
        }
        
    }
}