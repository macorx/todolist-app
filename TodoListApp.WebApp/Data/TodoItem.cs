using System;

namespace TodoListApp.WebApp.Data
{
    public class TodoItem
    {
        public string Id { get; protected set; }
        
        public string UserId { get; protected set; }

        public bool IsComplete { get; protected set; }
        public DateTime DateOfCreation { get; protected set; }
        public DateTime? DateOfLastUpdate { get; protected set; }
        public string Description { get; protected set; }

        private TodoItem()
        {
            
        }
        
        public TodoItem(string userId, string description)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            DateOfCreation = DateTime.Now;
            Description = description;
        }
        
        

    }
}