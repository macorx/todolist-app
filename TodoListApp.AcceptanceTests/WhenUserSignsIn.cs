using NUnit.Framework;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserSignsIn : AcceptanceTestBase
    {
        [Test]
        public void UserCanSignOut()
        {
            AuthenticateWithDefaultUser();

            ClickOnButtonWithId("logout");

            Assert.That(Driver.Url, Is.EqualTo(LoginUrl));
        }
    }
}