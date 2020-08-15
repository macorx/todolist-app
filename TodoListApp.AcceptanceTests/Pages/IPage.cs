using OpenQA.Selenium;

namespace TodoListApp.AcceptanceTests.Pages
{
    public interface IPage
    {
        IWebDriver Driver { get; }        
    }
}