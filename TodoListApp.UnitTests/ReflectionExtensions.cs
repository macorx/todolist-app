using System;

namespace TodoListApp.UnitTests
{
    public static class ReflectionExtensions
    {
        public static void SetProperty(this object obj, string propertyName, object value)
        {
            var type = obj.GetType();
            var property = type.GetProperty(propertyName);
            
            if (property == null)
                throw new InvalidOperationException($"Property {propertyName} does not exist on {type.Name}");
            
            property.SetValue(obj, value);
        }
    }
}