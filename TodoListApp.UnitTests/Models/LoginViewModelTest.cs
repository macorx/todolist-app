using NUnit.Framework;
using TodoListApp.WebApp.Models;

namespace TodoListApp.UnitTests.Models
{
    public class LoginViewModelTest : ViewModelTestBase<LoginViewModel>
    {
        [Test]
        public void UserNameIsRequired()
        {
            AssertPropertyIsRequired(nameof(LoginViewModel.UserName));            
        }

        [Test]
        public void PasswordIsRequired()
        {
            AssertPropertyIsRequired(nameof(LoginViewModel.Password));
        }
    }
}