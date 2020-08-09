using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models
{
    public class NewTodoItemViewModel
    {
        [Required]
        public string Description { get; set; }
    }
}