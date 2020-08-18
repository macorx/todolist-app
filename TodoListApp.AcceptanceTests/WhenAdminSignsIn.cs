using NUnit.Framework;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class WhenAdminSignsIn : AcceptanceTestBase
    {
        private AdminPage adminPage;

        [SetUp]
        public void SetUp()
        {
            adminPage = SignInAsAdmin();
        }
        
        [TearDown]
        public void TearDown()
        {
            adminPage.SignOut();
        }        

        [Test]
        public void DisplaysTotalUsers()
        {
            Assert.That(adminPage.TotalUsers, Is.EqualTo(3));
        }

        [Test]
        public void DisplaysTotalPendingTasks()
        {
            Assert.That(adminPage.TotalPendingTasks, Is.EqualTo(0));            
        }
        
        [Test]
        public void DisplaysTotalCompletedTasks()
        {
            Assert.That(adminPage.TotalCompletedTasks, Is.EqualTo(0));            
        }
        
        private AdminPage SignInAsAdmin()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            
            return new SignInPage(Driver)
                .WithUserName("admin")
                .WithPassword("pwd123")
                .SignIn<AdminPage>();
        }
        
    }
}