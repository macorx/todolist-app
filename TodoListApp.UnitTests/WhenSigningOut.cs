using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using TodoListApp.WebApp.Controllers;

namespace TodoListApp.UnitTests
{
    public class WhenSigningOut
    {
        private AccountController controller;
        private Mock<SignInManager<IdentityUser>> signInManager;

        [SetUp]
        public void SetUp()
        {
            signInManager = MockHelpers.ForSignInManager();
            controller = new AccountController(signInManager.Object);
        }

        [Test]
        public void CallsSignOutFromSignInManager()
        {
            controller.Logout();
            
            signInManager.Verify(s => s.SignOutAsync());
        }

    }
}