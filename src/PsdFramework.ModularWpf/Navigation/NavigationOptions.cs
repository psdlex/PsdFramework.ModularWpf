using PsdFramework.ModularWpf.Internal;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;

namespace PsdFramework.ModularWpf.Navigation;

public sealed class NavigationOptions
{
    private readonly OptionsBuildingManager _optionsBuildingManager = new();

    private NavigationOptions()
    {
    }

    internal Type? NavigationHostType { get; private set; }
    internal Type? NavigatableType { get; private set; }

    internal INavigationHost? NavigationHost { get; private set; }
    internal INavigatable? Navigatable { get; private set; }

    internal object? Category { get; private set; }

    internal bool IsNavigatableSet => _optionsBuildingManager.GetValue<bool>(nameof(IsNavigatableSet));
    internal Action<ContextualParametersBuilder>? ParametersBuilderConfiguration
        => _optionsBuildingManager.GetValue<Action<ContextualParametersBuilder>>(nameof(ParametersBuilderConfiguration));

    public static NavigationOptions FromNavigationHost(Type navigationHostType) => new() { NavigationHostType = navigationHostType };
    public static NavigationOptions FromNavigationHost(INavigationHost navigationHost) => new() { NavigationHost = navigationHost };
    public static NavigationOptions FromNavigationHost<TNavigationHost>() where TNavigationHost : class, INavigationHost
        => FromNavigationHost(typeof(TNavigationHost));

    public static NavigationOptions FromCategory(object category) => new() { Category = category };

    public NavigationOptions ToNavigatable(Type navigatableType)
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(IsNavigatableSet), true);

        NavigatableType = navigatableType;
        return this;
    }

    public NavigationOptions ToNavigatable(INavigatable navigatable)
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(IsNavigatableSet), true);

        Navigatable = navigatable;
        return this;
    }

    public NavigationOptions ToNavigatable<TNavigatable>() where TNavigatable : INavigatable
        => ToNavigatable(typeof(TNavigatable));

    public NavigationOptions WithParameters(Action<ContextualParametersBuilder> parametersConfiguration)
    {
        _optionsBuildingManager.ThrowOrContinueWithPropertyValue(nameof(ParametersBuilderConfiguration), parametersConfiguration);
        return this;
    }
}
