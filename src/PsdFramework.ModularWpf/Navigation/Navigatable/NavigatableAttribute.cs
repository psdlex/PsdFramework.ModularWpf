using PsdFramework.ModularWpf.General;

namespace PsdFramework.ModularWpf.Navigation.Navigatable;

public sealed class NavigatableAttribute : ComponentAttribute
{
    public string? DisplayName { get; init; }
}