using System;
using System.Linq;
using TodoListApp.WebApp.Data;

namespace TodoListApp.WebApp.Models
{
    public class TodoItemsViewModel
    {
        public IQueryable<TodoItem> Items { get; set; }
    }
}