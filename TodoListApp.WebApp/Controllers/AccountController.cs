using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl }); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) 
                return View(loginViewModel);

            var user = await signInManager.UserManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                    return await RedirectAuthenticatedUser(loginViewModel, user);
            }
                
            ModelState.AddModelError(string.Empty, "Couldn't login with user and password.");

            return View(loginViewModel);
        }

        private async Task<IActionResult> RedirectAuthenticatedUser(LoginViewModel loginViewModel, IdentityUser user)
        {
            if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl) && Url.IsLocalUrl(loginViewModel.ReturnUrl))
                return Redirect(loginViewModel.ReturnUrl);
            
            if (await IsAdmin(user))
                return RedirectToAction("Index", "Admin");

            return RedirectToAction("Index", "TodoItems");
        }

        private async Task<bool> IsAdmin(IdentityUser user)
        {
            return await signInManager.UserManager.IsInRoleAsync(user, "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }        
        
    }
}