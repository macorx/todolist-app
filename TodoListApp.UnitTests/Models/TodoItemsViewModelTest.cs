using NUnit.Framework;
using TodoListApp.WebApp.Models;

namespace TodoListApp.UnitTests.Models
{
    public class NewTodoItemViewModelTest : ViewModelTestBase<NewTodoItemViewModel>
    {
        [Test]
        public void DescriptionIsRequired()
        {
            AssertPropertyIsRequired(nameof(NewTodoItemViewModel.Description));
        }
    }
}