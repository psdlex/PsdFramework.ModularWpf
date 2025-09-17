using PsdFramework.ModularWpf.General.Models.Components;

namespace PsdFramework.ModularWpf.Navigations.Models.Navigation;

public sealed class NavigationComponentModelAttribute : ComponentModelAttribute
{
    public NavigationComponentModelAttribute(object category)
    {
        Category = category;
    }

    public object Category { get; }
}