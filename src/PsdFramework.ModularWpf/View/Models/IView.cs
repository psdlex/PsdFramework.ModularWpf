using System.Windows;

namespace PsdFramework.ModularWpf.View.Models;

public interface IView<TView>
    where TView : FrameworkElement
{
    TView View { get; }
}

public interface IView<TView, TModel> : IView<TView>
    where TView : FrameworkElement
{
    TModel Model { get; }
}