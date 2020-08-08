using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace TodoListApp.UnitTests
{
    public static class ObjectTestExtensions
    {
        public static T As<T>(this object obj)
        {
            Assert.IsInstanceOf<T>(obj);
            return (T) obj;
        }
    }
}