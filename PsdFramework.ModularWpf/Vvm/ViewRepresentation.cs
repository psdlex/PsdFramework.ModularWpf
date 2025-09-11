using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

namespace PsdFramework.ModularWpf.Vvm;

public sealed class ViewRepresentation<TView, TViewModel> : IView<TView, TViewModel>
    where TView : Control
    where TViewModel : ObservableObject
{
    public ViewRepresentation(TView view, TViewModel viewModel)
    {
        View = view;
        ViewModel = viewModel;
    }

    public TView View { get; }
    public TViewModel ViewModel { get; }
}