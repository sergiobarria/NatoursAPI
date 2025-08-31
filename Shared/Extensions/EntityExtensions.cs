using System.Reflection;

namespace Shared.Extensions;

public static class EntityExtensions
{
    public static void CopyPropertiesFrom<T>(this T target, T source, params string[] excludedProperties)
        where T : class
    {
        var sourceType = source.GetType();
        var targetType = target.GetType();

        foreach (var sourceProperty in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!sourceProperty.CanRead || excludedProperties.Contains(sourceProperty.Name)) continue;

            var targetProperty =
                targetType.GetProperty(sourceProperty.Name, BindingFlags.Public | BindingFlags.Instance);

            if (targetProperty == null || !targetProperty.CanWrite ||
                targetProperty.PropertyType != sourceProperty.PropertyType) continue;

            var value = sourceProperty.GetValue(source);
            targetProperty.SetValue(target, value);
        }
    }
}