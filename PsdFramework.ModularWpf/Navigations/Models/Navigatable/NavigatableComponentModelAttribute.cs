using PsdFramework.ModularWpf.General.Models.Components;

namespace PsdFramework.ModularWpf.Navigations.Models.Navigatable;

public sealed class NavigatableComponentModelAttribute : ComponentModelAttribute
{
    public NavigatableComponentModelAttribute(params object[] categories)
    {
        Categories = categories;
    }

    public object[] Categories { get; init; }
    public string? DisplayName { get; init; }
}