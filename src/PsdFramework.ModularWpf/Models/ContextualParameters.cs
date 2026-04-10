using Microsoft.Extensions.Logging.Abstractions;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace PsdFramework.ModularWpf.Models;

public sealed class ContextualParameters
{
    private readonly IReadOnlyDictionary<object, object?> _parameters;
    private readonly IReadOnlyCollection<object> _keylessParameters;

    internal ContextualParameters(IReadOnlyDictionary<object, object?> parameters, IReadOnlyCollection<object> keylessParameters)
    {
        _parameters = parameters;
        _keylessParameters = keylessParameters;
    }

    public object? this[string key] => _parameters[key];
    internal static ContextualParameters Empty() => new(new Dictionary<object, object?>(0), []);


    public bool TryGetValue<T>(object key, [NotNullWhen(true)] out T? value)
    {
        value = default;
        
        var success = _parameters.TryGetValue(key, out var boxedValue);
        if (success == false)
            return false;

        if (boxedValue is not T)
            throw new InvalidCastException($"Parameter with key '{key}' is not '{typeof(T)}'.");

        value = (T)boxedValue;
        return true;
    }

    public bool TryGetValue<T>([NotNullWhen(true)] out T? value)
    {
        value = default;

        var parameter = _keylessParameters.FirstOrDefault(p => p.GetType() == typeof(T));
        if (parameter is null)
            return false;

        value = (T)parameter;
        return true;
    }

    public T GetValue<T>(object key)
    {
        if (_parameters.TryGetValue(key, out var value) == false)
            throw new KeyNotFoundException($"Parameter with key '{key}' was not found.");

        if (value is not T typedValue)
            throw new InvalidCastException($"Parameter with key '{key}' is not '{typeof(T)}'.");

        return typedValue;
    }

    public T GetValue<T>()
    {
        if (_keylessParameters.FirstOrDefault(p => p.GetType() == typeof(T)) is not T typedValue)
            throw new KeyNotFoundException($"Keyless parameter with type '{typeof(T)}' was not found.");

        return typedValue;
    }

}