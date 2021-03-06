﻿using System;
using NUnit.Framework;
using TodoListApp.AcceptanceTests.Pages;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserDeletesItem : AcceptanceTestBase
    {
        private TodoListPage todoListPage;

        [SetUp]
        public void SetUp()
        {
            todoListPage = SignIn();
        }

        [Test]
        public void ItemsIsRemovedFromTodoList()
        {
            var currentCount = todoListPage.CountItems();
            var description = "Item " + DateTime.Now.ToString("s");
            AssumeExistingItemWithDescription(description);

            todoListPage.DeleteItemWithDescription(description);

            Assert.That(todoListPage.CountItems(), Is.EqualTo(currentCount));
        }

        private void AssumeExistingItemWithDescription(string description)
        {
            todoListPage.AddItem()
                .WithDescription(description)
                .Submit();
        }

        protected override void AdditionalTearDown()
        {
            todoListPage.SignOut();
        }
    }
}