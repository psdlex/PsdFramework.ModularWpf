using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace PsdFramework.ModularWpf.Popup.Models;

public interface IValidatableDataContext
{
    IRelayCommand<DependencyObject> RegisterValidatableContainerCommand { get; }
}