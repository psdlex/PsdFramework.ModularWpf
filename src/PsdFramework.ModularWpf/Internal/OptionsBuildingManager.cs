using System.Runtime.CompilerServices;

namespace PsdFramework.ModularWpf.Internal;

internal sealed class OptionsBuildingManager
{
    private readonly Dictionary<string, object> _propertyValueMap = [];

    public void ThrowOrContinueWithPropertyValue(string propertyName, object value, [CallerMemberName] string? methodName = default)
    {
        if (_propertyValueMap.ContainsKey(propertyName))
        {
            if (methodName is not null)
                throw new InvalidOperationException($"Configuration through method '{methodName}' is already used.");
            else
                throw new InvalidOperationException($"Property '{propertyName}' is already set.");
        }

        _propertyValueMap[propertyName] = value;
    }

    public T? GetValue<T>(string propertyName)
        => _propertyValueMap.TryGetValue(propertyName, out var value) && value is T typedValue ? typedValue : default;
}
