using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoListApp.WebApp.Controllers;
using TodoListApp.WebApp.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace TodoListApp.UnitTests
{
    public class WhenSigningIn
    {
        private AccountController controller;
        private Mock<SignInManager<IdentityUser>> signInManager;
        private Mock<IUrlHelper> urlHelper;

        [SetUp]
        public void SetUp()
        {
            signInManager = MockHelpers.ForSignInManager();

            AssumeAuthenticationResults(SignInResult.Success);
            
            controller = new AccountController(signInManager.Object);
            urlHelper = new Mock<IUrlHelper>();
            controller.Url = urlHelper.Object;
        }

        private void AssumeAuthenticationResults(SignInResult signInResult)
        {
            signInManager.Reset();
            signInManager.Setup(s =>
                    s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(signInResult));
        }

        [Test]
        public void ReturnsViewWhenAccessingLogin()
        {
            Assert.IsInstanceOf<ViewResult>(controller.Login(string.Empty));
        }

        [Test]
        public void ReturnsViewWithReturnUrlWhenAccessingLogin()
        {
            const string returnUrl = "Return-To-Url";
            var viewResult = controller.Login(returnUrl);
            Assert.IsInstanceOf<ViewResult>(viewResult);

            var viewModel = (LoginViewModel)((ViewResult) viewResult).Model;
            Assert.That(viewModel.ReturnUrl, Is.EqualTo(returnUrl));
        }

        [Test]
        public async Task AndModelIsInvalidReturnsToViewWithModel()
        {
            controller.ModelState.AddModelError("UserName","UserName is required");
            
            var loginViewModel = new LoginViewModel { Password = "password" };
            var result = await controller.Login(loginViewModel);
            
            AssertLoginViewModel(result.As<ViewResult>().Model.As<LoginViewModel>(), loginViewModel);
        }

        [Test]
        public async Task AuthenticatesUsingSignInManager()
        {
            var loginViewModel = GivenValidLoginCredentials();
            await controller.Login(loginViewModel);
            
            signInManager.Verify(s => 
                s.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false));
        }

        private static LoginViewModel GivenValidLoginCredentials(string returnUrl = "")
        {
            return new LoginViewModel { UserName = "test", Password = "password", ReturnUrl = returnUrl};
        }

        [Test]
        public async Task AndAuthenticationSucceedRedirectsToUsersTodoItems()
        {
            var result = await controller.Login(GivenValidLoginCredentials());

            AssertRedirectsToTodoItems(result);
        }

        private static void AssertRedirectsToTodoItems(IActionResult result)
        {
            var redirectToAction = result.As<RedirectToActionResult>();
            Assert.That(redirectToAction.ControllerName, Is.EqualTo("TodoItems"));
            Assert.That(redirectToAction.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task AndAuthenticationSucceedRedirectToReturnUrl()
        {
            const string returnUrl = "Return-To-Url";

            AssumeReturnUrlIs(returnUrl, local: true);
            
            var result = await controller.Login(GivenValidLoginCredentials(returnUrl: returnUrl));

            var redirectResult = result.As<RedirectResult>();
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
        }
        
        [Test]
        public async Task AndAuthenticationSucceedAndReturnUrlIsInvalidRedirectsToUsersTodoItems()
        {
            const string returnUrl = "http://invalid.com";

            AssumeReturnUrlIs(returnUrl, local: false);
            
            var result = await controller.Login(GivenValidLoginCredentials(returnUrl: returnUrl));

            AssertRedirectsToTodoItems(result);
        }

        private void AssumeReturnUrlIs(string returnUrl, bool local)
        {
            urlHelper.Setup(u => u.IsLocalUrl(returnUrl)).Returns(local);
        }

        [Test]
        public async Task AndAuthenticationFailReturnsToViewAndAddModelError()
        {
            AssumeAuthenticationResults(SignInResult.Failed);

            var loginViewModel = GivenInvalidCredentials();
            var result = await controller.Login(loginViewModel);

            CollectionAssert.AreEquivalent(new [] { "Couldn't login with user and password." }, GetErrorMessagesFromModelState());

            AssertLoginViewModel(result.As<ViewResult>().Model.As<LoginViewModel>(), loginViewModel);
        }

        private static void AssertLoginViewModel(LoginViewModel result, LoginViewModel loginViewModel)
        {
            Assert.That(result.UserName, Is.EqualTo(loginViewModel.UserName));
            Assert.That(result.ReturnUrl, Is.EqualTo(loginViewModel.ReturnUrl));
        }

        private IEnumerable<string> GetErrorMessagesFromModelState()
        {
            return controller.ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage);
        }

        private LoginViewModel GivenInvalidCredentials()
        {
            return new LoginViewModel() { UserName = "invalid-user", Password = "invalid-password" };
        }

    }
}