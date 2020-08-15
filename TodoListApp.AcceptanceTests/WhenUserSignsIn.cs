using NUnit.Framework;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserSignsIn : AcceptanceTestBase
    {
        [Test]
        public void UserCanSignOut()
        {
            var signedPage = SignIn();
            
            signedPage.SignOut();

            Assert.That(Driver.Url, Is.EqualTo(LoginUrl));
        }
    }
}