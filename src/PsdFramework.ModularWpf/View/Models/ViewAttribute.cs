using PsdFramework.ModularWpf.General;
using System.Windows;

namespace PsdFramework.ModularWpf.View.Models;

public class ViewAttribute : ComponentAttribute
{
    public ViewAttribute(Type viewType)
    {
        if (viewType.IsAssignableTo(typeof(FrameworkElement)) == false)
            throw new ArgumentException($"View '{viewType}' must be a {nameof(FrameworkElement)}.");

        if (viewType.GetConstructors().Any(c => c.GetParameters().Length == 0) == false)
            throw new ArgumentException($"View '{viewType}' must have an empty constructor.");

        ViewType = viewType;
    }

    public Type ViewType { get; }
}

public sealed class ViewAttribute<TView>() : ViewAttribute(typeof(TView))
    where TView : FrameworkElement, new();