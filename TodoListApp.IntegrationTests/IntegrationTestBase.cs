using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TodoListApp.WebApp;
using TodoListApp.WebApp.Data;

namespace TodoListApp.IntegrationTests
{
    public class IntegrationTestBase
    {
        private IServiceScope scope;
        private ApplicationDbContext dbContext;
        protected WebApplicationFactory<Startup> Factory { get; private set; }
        protected IServiceProvider ServiceProvider => scope.ServiceProvider;
        
        
        [SetUp]
        public void Setup()
        {
            Factory = new WebApplicationFactory<Startup>();
            scope = Factory.Services.CreateScope();
            AdditionalSetup();
        }

        protected virtual void AdditionalSetup()
        {

        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
            
            scope.Dispose();
        }

        private void CleanUp()
        {
            dbContext = ServiceProvider.GetService<ApplicationDbContext>();
            dbContext.RemoveRange(dbContext.TodoItems);
            dbContext.SaveChanges();
        }
    }
}