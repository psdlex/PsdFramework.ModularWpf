using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;

namespace PsdFramework.ModularWpf.Navigation.Service;

public sealed class NavigationOptions
{
    private bool _areParametersConfigured;

    private NavigationOptions()
    {
    }

    internal bool IsNavigatableSet { get; private set; }

    internal Type? NavigationHostType { get; private set; }
    internal Type? NavigatableType { get; private set; }

    internal INavigationHost? NavigationHost { get; private set; }
    internal INavigatable? Navigatable { get; private set; }

    internal object? Category { get; private set; }
    internal Action<ContextualParametersBuilder>? ParametersBuilderConfiguration { get; private set; }

    public static NavigationOptions FromNavigationHost(Type navigationHostType) => new() { NavigationHostType = navigationHostType };
    public static NavigationOptions FromNavigationHost(INavigationHost navigationHost) => new() { NavigationHost = navigationHost };
    public static NavigationOptions FromNavigationHost<TNavigationHost>() where TNavigationHost : class, INavigationHost
        => FromNavigationHost(typeof(TNavigationHost));

    public static NavigationOptions FromCategory(object category) => new() { Category = category };

    public NavigationOptions ToNavigatable(Type navigatableType)
    {
        if (IsNavigatableSet)
            throw new InvalidOperationException("Navigatable is already set.");

        IsNavigatableSet = true;
        NavigatableType = navigatableType;
        return this;
    }

    public NavigationOptions ToNavigatable(INavigatable navigatable)
    {
        if (IsNavigatableSet)
            throw new InvalidOperationException("Navigatable is already set.");

        IsNavigatableSet = true;
        Navigatable = navigatable;
        return this;
    }

    public NavigationOptions ToNavigatable<TNavigatable>() where TNavigatable : INavigatable
        => ToNavigatable(typeof(TNavigatable));

    public NavigationOptions WithParameters(Action<ContextualParametersBuilder> parametersConfiguration)
    {
        if (_areParametersConfigured)
            throw new InvalidOperationException("Parameters are already set.");

        _areParametersConfigured = true;
        ParametersBuilderConfiguration = parametersConfiguration;
        return this;
    }
}
