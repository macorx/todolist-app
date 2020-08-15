using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public interface ISignedPage : IPage
    {

    }
    
    public static class SignedPageExtension
    {
        public static SignInPage SignOut(this ISignedPage page)
        {
            page.Driver.FindElement(By.Id("logout")).Click();            
            return new SignInPage(page.Driver);
        }
        
    }
    
}