using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests
{
    public class TodoItem
    {
        public int Index { get; }
        public DateTime DateOfCreation { get; } 
        public DateTime? DateOfLastUpdate { get; } 
        public string Description { get; }
        public string State { get; }

        public TodoItem(int index, ReadOnlyCollection<IWebElement> columns)
        {
            Index = index;
            State = columns[0].Text;
            Description = columns[1].Text;            
            DateOfCreation = DateTime.Parse(columns[2].Text);
            
            if (!string.IsNullOrEmpty(columns[3].Text))
                DateOfLastUpdate =  DateTime.Parse(columns[3].Text);
        }
    }
}