using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace PsdFramework.ModularWpf.Popup.Models;

public interface IValidatableDataContext
{
    IRelayCommand<DependencyObject> RegisterValidatableContainerCommand { get; }
}