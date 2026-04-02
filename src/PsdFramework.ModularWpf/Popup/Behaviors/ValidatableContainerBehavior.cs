using System.Windows;
using PsdFramework.ModularWpf.Popup.Models;

namespace PsdFramework.ModularWpf.Popup.Behaviors;

public static class ValidatableContainerBehavior
{
    public static readonly DependencyProperty DataContextProperty =
        DependencyProperty.RegisterAttached(
            "DataContext",
            typeof(IValidatableDataContext),
            typeof(ValidatableContainerBehavior),
            new PropertyMetadata(default, OnDataContextChanged)
        );

    public static IValidatableDataContext GetDataContext(DependencyObject d) => (IValidatableDataContext)d.GetValue(DataContextProperty);
    public static void SetDataContext(DependencyObject d, IValidatableDataContext value) => d.SetValue(DataContextProperty, value);

    private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element && e.NewValue is IValidatableDataContext dataContext)
            dataContext.RegisterValidatableContainerCommand.Execute(element);
    }
}