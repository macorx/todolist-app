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

namespace TodoListApp.UnitTests.Controllers
{
    public class WhenSigningIn
    {
        private AccountController controller;
        private Mock<SignInManager<IdentityUser>> signInManager;
        private Mock<IUrlHelper> urlHelper;
        private Mock<UserManager<IdentityUser>> userManager;

        [SetUp]
        public void SetUp()
        {
            userManager = MockHelpers.ForUserManager();
            signInManager = MockHelpers.ForSignInManager(userManager);

            controller = new AccountController(signInManager.Object);
            urlHelper = new Mock<IUrlHelper>();
            controller.Url = urlHelper.Object;
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
            var identityUser = GivenExistingUser(loginViewModel.UserName);
            AssumeAuthenticationResults(identityUser, SignInResult.Success);

            await controller.Login(loginViewModel);
            
            signInManager.Verify(s => s.PasswordSignInAsync(identityUser, loginViewModel.Password, false, false));
        }

        [Test]
        public async Task AndAuthenticationSucceedRedirectsToTodoItemsPage()
        {
            var loginViewModel = GivenValidLoginCredentials(userName: "user");            
            var identityUser = GivenExistingUser(loginViewModel.UserName, "User");
            AssumeAuthenticationResults(identityUser, SignInResult.Success);
            
            var result = await controller.Login(loginViewModel);

            AssertRedirectsTo(result, "TodoItems", "Index");            
        }

        [Test]
        public async Task AndUserIsAdminAndAuthenticationSucceedRedirectsToAdminPage()
        {
            var loginViewModel = GivenValidLoginCredentials(userName: "admin");            
            var identityUser = GivenExistingUser(loginViewModel.UserName, "Admin");
            AssumeAuthenticationResults(identityUser, SignInResult.Success);
            
            var result = await controller.Login(loginViewModel);

            AssertRedirectsTo(result, "Admin", "Index");
        }

        [TestCase("user","User")]
        [TestCase("admin","Admin")]
        public async Task AndAuthenticationSucceedRedirectToReturnUrl(string userName, string role)
        {
            const string returnUrl = "Return-To-Url";
            AssumeReturnUrlIs(returnUrl, local: true);
            var identityUser = GivenExistingUser(userName, role);
            AssumeAuthenticationResults(identityUser, SignInResult.Success);
            
            var result = await controller.Login(GivenValidLoginCredentials(userName: userName, returnUrl: returnUrl));

            var redirectResult = result.As<RedirectResult>();
            Assert.That(redirectResult.Url, Is.EqualTo(returnUrl));
        }

        [TestCase("user","User", "TodoItems")]
        [TestCase("admin","Admin", "Admin")]
        public async Task AndAuthenticationSucceedAndReturnUrlIsInvalidRedirectsToUsersTodoItems(string userName, string role, string controllerName)
        {
            const string returnUrl = "http://invalid.com";
            AssumeReturnUrlIs(returnUrl, local: false);
            var identityUser = GivenExistingUser(userName, role);
            AssumeAuthenticationResults(identityUser, SignInResult.Success);
            
            var result = await controller.Login(GivenValidLoginCredentials(userName: userName, returnUrl: returnUrl));

            AssertRedirectsTo(result, controllerName, "Index");
        }

        [Test]
        public async Task AndUserDoesNotExistReturnsModelError()
        {
            var loginViewModel = GivenValidLoginCredentials(userName: "user-not-exist");
            
            var result = await controller.Login(loginViewModel);
            
            CollectionAssert.AreEquivalent(new [] { "Couldn't login with user and password." }, GetErrorMessagesFromModelState());
            AssertLoginViewModel(result.As<ViewResult>().Model.As<LoginViewModel>(), loginViewModel);            
        }

        [Test]
        public async Task AndPasswordIsNotValidReturnsModelError()
        {
            var loginViewModel = GivenInvalidCredentials();
            AssumeAuthenticationResults(GivenExistingUser(loginViewModel.UserName), SignInResult.Failed);

            var result = await controller.Login(loginViewModel);

            CollectionAssert.AreEquivalent(new [] { "Couldn't login with user and password." }, GetErrorMessagesFromModelState());
            AssertLoginViewModel(result.As<ViewResult>().Model.As<LoginViewModel>(), loginViewModel);
        }

        
        private static void AssertLoginViewModel(LoginViewModel result, LoginViewModel loginViewModel)
        {
            Assert.That(result.UserName, Is.EqualTo(loginViewModel.UserName));
            Assert.That(result.ReturnUrl, Is.EqualTo(loginViewModel.ReturnUrl));
        }
        
        private static void AssertRedirectsTo(IActionResult result, string controllerName, string actionName)
        {
            var redirectToAction = result.As<RedirectToActionResult>();
            Assert.That(redirectToAction.ControllerName, Is.EqualTo(controllerName));
            Assert.That(redirectToAction.ActionName, Is.EqualTo(actionName));
        }

        private void AssumeAuthenticationResults(IdentityUser identityUser, SignInResult signInResult)
        {
            signInManager.Setup(s =>
                    s.PasswordSignInAsync(identityUser, It.IsAny<string>(),
                        It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(signInResult));
        }

        private IdentityUser GivenExistingUser(string userName, string role = "user")
        {
            var identityUser = new IdentityUser(userName);
            userManager.Setup(u => u.FindByNameAsync(userName)).Returns(Task.FromResult(identityUser));
            userManager.Setup(u => u.IsInRoleAsync(identityUser, role)).Returns(Task.FromResult(true));
            return identityUser;
        }

        private void AssumeReturnUrlIs(string returnUrl, bool local)
        {
            urlHelper.Setup(u => u.IsLocalUrl(returnUrl)).Returns(local);
        }

        private IEnumerable<string> GetErrorMessagesFromModelState()
        {
            return controller.ModelState.SelectMany(m => m.Value.Errors).Select(e => e.ErrorMessage);
        }

        private static LoginViewModel GivenValidLoginCredentials(string userName = "test", string password = "password", string returnUrl = "")
        {
            return new LoginViewModel { UserName = userName, Password = password, ReturnUrl = returnUrl};
        }

        private LoginViewModel GivenInvalidCredentials()
        {
            return new LoginViewModel() { UserName = "invalid-user", Password = "invalid-password" };
        }
    }
}