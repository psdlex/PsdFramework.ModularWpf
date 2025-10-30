using System.Windows;
using PsdFramework.ModularWpf.PopupWindows.Models;

namespace PsdFramework.ModularWpf.PopupWindows.Behaviors;

public static class ValidationContainerBehavior
{
    public static readonly DependencyProperty DataContextProperty =
        DependencyProperty.RegisterAttached(
            "DataContext",
            typeof(IValidatableDataContext),
            typeof(ValidationContainerBehavior),
            new PropertyMetadata(default, OnDataContextChanged)
        );

    public static IValidatableDataContext GetDataContext(DependencyObject d) => (IValidatableDataContext)d.GetValue(DataContextProperty);
    public static void SetDataContext(DependencyObject d, IValidatableDataContext value) => d.SetValue(DataContextProperty, value);

    private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not FrameworkElement element || e.NewValue is not IValidatableDataContext dataContext)
            return;

        dataContext.RegisterValidationContainerCommand.Execute(element);
    }
}