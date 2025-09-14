namespace PsdFramework.ModularWpf.Navigations.Models.Navigatable;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class NavigatableModelAttribute : Attribute
{
    public NavigatableModelAttribute(params object[] categories)
    {
        Categories = categories;
    }

    public object[] Categories { get; }
}