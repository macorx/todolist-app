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

        public TodoListPage SignIn()
        {
            Driver.FindElement(By.Id("login")).Submit();
            return new TodoListPage(Driver);
        }
    }
}