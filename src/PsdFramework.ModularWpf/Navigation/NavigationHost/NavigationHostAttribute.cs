using PsdFramework.ModularWpf.General;

namespace PsdFramework.ModularWpf.Navigation.NavigationHost;

public sealed class NavigationHostAttribute : ComponentAttribute
{
    public object? Category { get; init; }
}