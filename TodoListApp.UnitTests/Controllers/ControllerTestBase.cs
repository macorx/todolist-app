using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TodoListApp.UnitTests.Controllers
{
    public class ControllerTestBase
    {
        protected void AssumeUserIdIsSignedIn(Controller controller, string userId)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = CreateClaimsPrincipalForUserId(userId)
                }
            };
        }

        private ClaimsPrincipal CreateClaimsPrincipalForUserId(string userId)
        {
            var identity = new ClaimsIdentity(new []
            {
                new Claim(ClaimTypes.NameIdentifier, userId)                
            }, "TestAuthType");
            
            return new ClaimsPrincipal(identity);            
        }        
        
    }
}