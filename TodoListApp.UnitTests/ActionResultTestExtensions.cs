using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace TodoListApp.UnitTests
{
    public static class ObjectTestExtensions
    {
        public static T As<T>(this object obj)
        {
            Assert.IsInstanceOf<T>(obj,$"Object is not of type {typeof(T).Name}");
            return (T) obj;
        }
    }
}