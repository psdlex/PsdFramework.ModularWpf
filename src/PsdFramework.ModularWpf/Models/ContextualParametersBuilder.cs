namespace PsdFramework.ModularWpf.Models;

public sealed partial class ContextualParametersBuilder
{
    private readonly Dictionary<object, object?> _parameters = [];
    private readonly HashSet<object> _keylessParameters = [];

    internal ContextualParametersBuilder()
    {
    }

    public void Add(object key, object? value) => _parameters.Add(key, value);
    public void Add(object keylessParameter)
    {
        if (_keylessParameters.Any(p => p.GetType() == keylessParameter.GetType()))
            throw new ArgumentException($"Keyless parameter with type '{keylessParameter.GetType()}' already exists.", nameof(keylessParameter));

        _keylessParameters.Add(keylessParameter);
    }

    internal ContextualParameters Build()
        => new(_parameters, _keylessParameters);
}
