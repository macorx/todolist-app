using System.Collections.Generic;
using NUnit.Framework;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class WhenAdminSignsIn : AcceptanceTestBase
    {
        private AdminPage adminPage;

        [OneTimeSetUp]
        public void SetUp()
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
        public void TearDown()
        {
            adminPage.SignOut();
        }

        [Test]
        public void DisplaysTodoItemsPerUser()
        {
            var expectedValues = new Dictionary<string, (int pending, int completed)>
            { {"test",(1,1)}, {"test2",(0,1)} };
            
            foreach (var (user, pending, completed) in adminPage.GetItemsPerUser())
            {
                Assert.Contains(user, expectedValues.Keys);
                Assert.That(expectedValues[user].pending, Is.EqualTo(pending));
                Assert.That(expectedValues[user].completed, Is.EqualTo(completed));
            }
        }

        [Test]
        public void DisplaysTotalUsers()
        {
            Assert.That(adminPage.TotalUsers, Is.EqualTo(3));
        }

        [Test]
        public void DisplaysTotalPendingTasks()
        {
            Assert.That(adminPage.TotalPendingTasks, Is.EqualTo(2));            
        }

        [Test]
        public void DisplaysTotalCompletedTasks()
        {
            Assert.That(adminPage.TotalCompletedTasks, Is.EqualTo(1));            
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