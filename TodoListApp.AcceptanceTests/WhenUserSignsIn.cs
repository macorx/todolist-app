using NUnit.Framework;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserSignsIn : AcceptanceTestBase
    {
        [Test]
        public void DisplaysUsersTodoListAndSignsOut()
        {
            AuthenticateWithDefaultUser();

            Assert.That(Driver.Url, Is.EqualTo($"{BasePath}/"));

            ClickOnButton("logout");

            Assert.That(Driver.Url, Is.EqualTo(LoginUrl));
        }
    }
}