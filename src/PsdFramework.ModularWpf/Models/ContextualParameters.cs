namespace PsdFramework.ModularWpf.Models;

public sealed class ContextualParameters
{
    private readonly Dictionary<object, object?> _parameters = [];
    private readonly HashSet<object> _keylessParameters = [];

    internal ContextualParameters()
    {
    }

    public void Add(object key, object? value) => _parameters.Add(key, value);
    public void Add(object keylessParameter)
    {
        if (_keylessParameters.Any(p => p.GetType() == keylessParameter.GetType()))
            throw new ArgumentException($"Keyless parameter with type '{keylessParameter.GetType()}' already exists.", nameof(keylessParameter));

        _keylessParameters.Add(keylessParameter);
    }

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

    public object? this[string key] => _parameters[key];

    public static readonly ContextualParameters Empty = new();
}