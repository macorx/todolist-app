using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace TodoListApp.UnitTests
{
    public static class MockHelpers
    {
        private static Mock<UserManager<IdentityUser>> ForUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Object.UserValidators.Add(new UserValidator<IdentityUser>());
            userManager.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());
            return userManager;
        }

        public static Mock<SignInManager<IdentityUser>> ForSignInManager()
        {
            var userManager = ForUserManager();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            var optionsAccessor = new Mock<IOptions<IdentityOptions>>();
            var logger = new Mock<ILogger<SignInManager<IdentityUser>>>();
            var schemes = new Mock<IAuthenticationSchemeProvider>();
            var confirmation = new Mock<IUserConfirmation<IdentityUser>>();

            return new Mock<SignInManager<IdentityUser>>(userManager.Object, contextAccessor.Object,
                claimsFactory.Object, optionsAccessor.Object, logger.Object, schemes.Object, confirmation.Object);
        }
    }
}