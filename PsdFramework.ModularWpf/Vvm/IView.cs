using System.Windows.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

namespace PsdFramework.ModularWpf.Vvm;

public interface IView<TView> where TView : Control
{
    TView View { get; }
}

public interface IView<TView, TViewModel> : IView<TView>
    where TView : Control
    where TViewModel : ObservableObject
{
    TViewModel ViewModel { get; }
}