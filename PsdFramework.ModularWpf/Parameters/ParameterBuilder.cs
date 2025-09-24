using System.Linq.Expressions;
using System.Reflection;

namespace PsdFramework.ModularWpf.Parameters;

public sealed class ParameterBuilder<T>
{
    private readonly Dictionary<PropertyInfo, object?> _parameters = [];

    internal ParameterBuilder()
    {
    }

    public ParameterBuilder<T> SetPropertyValue<TProperty>(Expression<Func<T, TProperty>> selector, TProperty value)
    {
        if (selector.Body is not MemberExpression memberExpr || memberExpr.Member is not PropertyInfo propInfo)
            throw new InvalidOperationException("Member is not a property");

        Utils.EnsurePropertyIntegrity(propInfo);

        _parameters.Add(propInfo, value);
        return this;
    }

    internal void ApplyPropertyValues(T instance)
    {
        foreach (var parameter in _parameters)
            parameter.Key.SetValue(instance, parameter.Value);
    }
}

public sealed class ParameterBuilder
{
    private readonly Type _targetType;
    private readonly Dictionary<PropertyInfo, object?> _parameters = [];

    internal ParameterBuilder(Type targetType)
    {
        _targetType = targetType ?? throw new ArgumentNullException(nameof(targetType));
    }

    public ParameterBuilder SetPropertyValue(string propertyName, object? value)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            throw new ArgumentException("Property name is required.", nameof(propertyName));

        var propInfo = _targetType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        Utils.EnsurePropertyIntegrity(propInfo);

        _parameters.Add(propInfo, value);
        return this;
    }

    internal void ApplyPropertyValues(object instance)
    {
        if (instance is null)
            throw new ArgumentNullException(nameof(instance));

        if (_targetType.IsInstanceOfType(instance) == false)
            throw new ArgumentException($"Instance is not of type '{_targetType.Name}'.");

        foreach (var parameter in _parameters)
            parameter.Key.SetValue(instance, parameter.Value);
    }
}
