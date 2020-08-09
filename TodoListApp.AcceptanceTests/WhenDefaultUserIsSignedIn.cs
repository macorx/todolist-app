﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests
{
    public class WhenDefaultUserIsSignedIn : AcceptanceTestBase
    {
        protected override void AdditionalSetup()
        {
            AuthenticateWithDefaultUser();
        }

        [Test]
        public void UserCanAddItemToHisTodoList()
        {
            const string itemDescription = "Review notes from last meeting.";
            FillInput("Description",itemDescription);
            ClickOn("Add");

            var items = Driver.FindElements(By.CssSelector("table tbody tr"));
            
            Assert.That(items.Count, Is.EqualTo(1));
            var todoItem = new TodoItem(items[0].FindElements(By.CssSelector("td")));

            Assert.That(todoItem.State, Is.EqualTo("Pending"));            
            Assert.That(todoItem.Description, Is.EqualTo(itemDescription));            
            Assert.That(todoItem.DateOfCreation, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(2)));
            Assert.That(todoItem.DateOfLastUpdate, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(2)));

        }
    }
}