using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TodoListApp.WebApp;

namespace TodoListApp.IntegrationTests
{
    public class IntegrationTestBase
    {
        private IServiceScope scope;
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
            scope.Dispose();
        }        
    }
}