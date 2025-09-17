using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

namespace PsdFramework.ModularWpf.Views.Models.Representation;

public interface IView<TView>
    where TView : FrameworkElement
{
    TView View { get; }
}

public interface IView<TView, TModel> : IView<TView>
    where TView : FrameworkElement
    where TModel : ObservableObject
{
    TModel Model { get; }
}