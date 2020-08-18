using System;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public class SignInPage : IPage
    {
        public IWebDriver Driver { get; }

        public SignInPage(IWebDriver driver)
        {
            Driver = driver;
        }

        public SignInPage WithUserName(string userName)
        {
            Driver.FindElement(By.Id("UserName")).SendKeys(userName);            
            return this;
        }

        public SignInPage WithPassword(string password)
        {
            Driver.FindElement(By.Id("Password")).SendKeys(password);            
            return this;
        }

        public TPage SignIn<TPage>() where TPage : ISignedPage
        {
            Driver.FindElement(By.Id("login")).Submit();
            return (TPage)Activator.CreateInstance(typeof(TPage), Driver);
        }
    }
}