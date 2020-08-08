using NUnit.Framework;
using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests
{
    public class WhenUserSignsIn : AcceptanceTestBase
    {
        [Test]
        public void DisplaysUsersTodoListAndSignsOut()
        {
            Driver.FindElement(By.CssSelector("input#UserName")).SendKeys("test");
            Driver.FindElement(By.CssSelector("input#Password")).SendKeys("pwd123");
            Driver.FindElement(By.CssSelector("button#login")).Submit();
            
            Driver.WaitUntilPageIsReady();
            
            Assert.That(Driver.Url, Is.EqualTo($"{BasePath}/"));
            
            Driver.FindElement(By.CssSelector("button#logout")).Submit();
            
            Assert.That(Driver.Url, Is.EqualTo(LoginUrl));            
        }
    }
}