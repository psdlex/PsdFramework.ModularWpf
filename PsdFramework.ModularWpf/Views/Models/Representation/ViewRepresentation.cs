using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

namespace PsdFramework.ModularWpf.Views.Models.Representation;

public class ViewRepresentation<TView> : IView<TView> 
    where TView : FrameworkElement
{
    public ViewRepresentation(TView view)
    {
        View = view;
    }

    public TView View { get; }
}

public sealed class ViewRepresentation<TView, TModel> : ViewRepresentation<TView>, IView<TView, TModel>
    where TView : FrameworkElement
    where TModel : ObservableObject
{
    public ViewRepresentation(TView view, TModel model) : base(view)
    {
        Model = model;
    }

    public TModel Model { get; }
}