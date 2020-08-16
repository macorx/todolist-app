using System;
using System.ComponentModel.DataAnnotations;
using TodoListApp.WebApp.Data;

namespace TodoListApp.WebApp.Models
{
    public class EditItemViewModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name="Is done?")]
        public bool IsDone { get; set; }

        public EditItemViewModel()
        {
            
        }

        public EditItemViewModel(TodoItem item)
        {
            Id = item.Id;
            Description = item.Description;
            IsDone = item.IsDone;
        }

        public void Update(TodoItem item)
        {
            item.Description = Description;
            item.IsDone = IsDone;
            item.DateOfLastUpdate = DateTime.Now;
        }
    }
}