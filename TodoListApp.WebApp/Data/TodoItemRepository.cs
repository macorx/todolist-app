using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoListApp.WebApp.Data
{
    public interface ITodoItemRepository
    {
        IQueryable<TodoItem> GetAll(string userId);
        Task Add(TodoItem todoItem);
        Task Delete(TodoItem todoItem);
        Task ApplyChanges();
    }

    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TodoItemRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public IQueryable<TodoItem> GetAll(string userId)
        {
            return dbContext.TodoItems.Where(t => t.UserId == userId).AsQueryable();
        }

        public async Task Add(TodoItem todoItem)
        {
            await dbContext.TodoItems.AddAsync(todoItem);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(TodoItem todoItem)
        {
            dbContext.TodoItems.Remove(todoItem);

            await dbContext.SaveChangesAsync();
        }

        public async Task ApplyChanges()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}