using System.Collections.Generic;
using NUnit.Framework;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class WhenAdminSignsIn : AcceptanceTestBase
    {
        private AdminPage adminPage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // test
            var todoListPage = SignIn(userName: "test");
            todoListPage.DeleteAllItems();
            
            CreateItemAndMarkAsDone(todoListPage, "one");
            CreateTodoItem(todoListPage, "two");            

            todoListPage.SignOut();
            
            
            // test2
            todoListPage = SignIn(userName: "test2");
            todoListPage.DeleteAllItems();
            
            CreateTodoItem(todoListPage, "three");

            todoListPage.SignOut();

            
            // admin
            adminPage = SignInAsAdmin();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            adminPage.SignOut();
        }

        [Test]
        public void DisplaysTodoItemsPerUser()
        {
            var expectedValues = new Dictionary<string, (int pending, int done)>
            { {"test",(1,1)}, {"test2",(1,0)} };
            
            foreach (var (user, pending, done) in adminPage.GetItemsPerUser())
            {
                CollectionAssert.Contains(expectedValues.Keys, user);
                Assert.That(expectedValues[user].pending, Is.EqualTo(pending), $"{user} has {pending} pending items");
                Assert.That(expectedValues[user].done, Is.EqualTo(done), $"{user} has {done} pending items");
            }
        }

        [Test]
        public void DisplaysTotalUsers()
        {
            Assert.That(adminPage.TotalUsers, Is.EqualTo(2));
        }

        [Test]
        public void DisplaysTotalPendingItems()
        {
            Assert.That(adminPage.TotalPendingItems, Is.EqualTo(2));            
        }

        [Test]
        public void DisplaysTotalDoneItems()
        {
            Assert.That(adminPage.TotalDoneItems, Is.EqualTo(1));            
        }

        private AdminPage SignInAsAdmin()
        {
            Driver.Navigate().GoToUrl(LoginUrl);
            
            return new SignInPage(Driver)
                .WithUserName("admin")
                .WithPassword("pwd123")
                .SignIn<AdminPage>();
        }

        private static void CreateTodoItem(TodoListPage todoListPage, string description)
        {
            todoListPage
                .AddItem()
                .WithDescription(description)
                .Submit();
        }

        private static void CreateItemAndMarkAsDone(TodoListPage todoListPage, string description)
        {
            todoListPage
                .AddItem()
                .WithDescription(description)
                .Submit();

            todoListPage
                .EditItemWithDescription(description)
                .MarkAsDone()
                .Submit();
        }
    }
}