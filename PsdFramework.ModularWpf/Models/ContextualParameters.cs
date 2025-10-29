namespace PsdFramework.ModularWpf.Models;

public sealed class ContextualParameters
{
    private readonly Dictionary<string, object?> _parameters = [];

    internal ContextualParameters()
    {
    }

    public void Add(string key, object? value) => _parameters.Add(key, value);

    public T? TryGetValue<T>(string key)
    {
        return _parameters.TryGetValue(key, out var value) && value is T typedValue 
            ? typedValue 
            : default!;
    }

    public T GetValue<T>(string key)
    {
        if (!_parameters.TryGetValue(key, out var value))
            throw new KeyNotFoundException($"The parameter with key '{key}' was not found.");

        if (value is not T typedValue)
            throw new InvalidCastException($"The parameter with key '{key}' is not of type '{typeof(T).FullName}'.");

        return typedValue;
    }

    public object? this[string key] => _parameters[key];
}