using System;
using TodoListApp.WebApp.Data;

namespace TodoListApp.UnitTests.Controllers
{
    public class TodoItemBuilder
    {
        private string id;
        private string userId;
        private string description;
        private bool? isDone;
        private DateTime? dateOfCreation;
        private DateTime? dateOfLastUpdate;

        public TodoItemBuilder WithUserId(string userId)
        {
            this.userId = userId;
            return this;
        }

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
            todoItem.SetPropertyIfValue(nameof(TodoItem.IsDone), dateOfCreation);
            todoItem.SetPropertyIfValue(nameof(TodoItem.IsDone), dateOfLastUpdate);
            return todoItem;
        }
    }
}