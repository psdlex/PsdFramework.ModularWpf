namespace PsdFramework.ModularWpf.Navigations.Models.Navigation;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class NavigationModelAttribute : Attribute
{
    public NavigationModelAttribute(object category)
    {
        Category = category;
    }

    public object Category { get; }
}