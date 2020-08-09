using System.ComponentModel.DataAnnotations;
using System.Reflection;
using NUnit.Framework;

namespace TodoListApp.UnitTests.Models
{
    public class ViewModelTestBase<TViewModel>
    {
        protected void AssertPropertyIsRequired(string propertyName)
        {
            var property = typeof(TViewModel).GetProperty(propertyName);

            Assert.IsNotNull(property, $"{propertyName} does not exist on {typeof(TViewModel).Name} ");
            Assert.IsNotNull(property.GetCustomAttribute<RequiredAttribute>(), $"Property {propertyName} is not marked as required.");
        } 
    }
}