namespace PsdFramework.ModularWpf.General.Models.Components;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class ComponentModelAttribute : Attribute
{
    public bool IsCached { get; set; }
}