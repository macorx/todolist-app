using System;
using TodoListApp.WebApp.Data;

namespace TodoListApp.UnitTests.Controllers
{
    public class TodoItemBuilder
    {
        private string id;
        private readonly string userId = "e40e5fb2-9887-495a-a4bc-910e492f7eb5";
        private string description;
        private bool? isDone;

        public TodoItemBuilder WithDescription(string description)
        {
            this.description = description;
            return this;
        }

        public TodoItemBuilder WithIsDone(bool isDone)
        {
            this.isDone = isDone;
            return this;
        }

        public TodoItemBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public TodoItem Build()
        {
            var todoItem = new TodoItem(userId, description);
            todoItem.SetPropertyIfValue(nameof(TodoItem.Id), id);
            todoItem.SetPropertyIfValue(nameof(TodoItem.IsDone), isDone);
            return todoItem;
        }
    }
}