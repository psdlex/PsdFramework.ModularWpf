namespace PsdFramework.ModularWpf.Navigations.Models;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class NavigationModelAttribute : Attribute
{
    public NavigationModelAttribute(object navigationModelId)
    {
        NavigationModelId = navigationModelId;
    }

    public object NavigationModelId { get; }
    public bool RegisterAsSeperateService { get; set; } = false;
}