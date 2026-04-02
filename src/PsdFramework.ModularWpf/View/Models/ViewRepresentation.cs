using System.Windows;

namespace PsdFramework.ModularWpf.View.Models;

internal sealed class ViewRepresentation<TView, TModel> : IView<TView, TModel>
    where TView : FrameworkElement
{
    public ViewRepresentation(TView view, TModel model)
    {
        View = view;
        Model = model;
    }

    public TView View { get; }
    public TModel Model { get; }
}