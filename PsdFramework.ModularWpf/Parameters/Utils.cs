using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PsdFramework.ModularWpf.Parameters;

#pragma warning disable CS8763
internal static class Utils
{
    [DoesNotReturn]
    public static void EnsurePropertyIntegrity(PropertyInfo? propertyInfo)
    {
        if (propertyInfo is null)
            throw new InvalidOperationException("Property does not exist.");

        if (propertyInfo.CanWrite == false)
                throw new InvalidOperationException("Property must not be Read-Only.");

        if (propertyInfo.GetCustomAttribute<ParameterBuilderPropertyAttribute>() is null)
            throw new InvalidOperationException($"Property must have a '{nameof(ParameterBuilderPropertyAttribute)}' attribute.");
    }
}