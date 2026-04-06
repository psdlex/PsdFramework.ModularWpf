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


    public T? TryGetValue<T>(object key)
    {
        return _parameters.TryGetValue(key, out var value) && value is T typedValue
            ? typedValue
            : default;
    }

    public T? TryGetValue<T>()
    {
        return _keylessParameters.FirstOrDefault(p => p.GetType() == typeof(T)) is T typedValue
            ? typedValue
            : default;
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