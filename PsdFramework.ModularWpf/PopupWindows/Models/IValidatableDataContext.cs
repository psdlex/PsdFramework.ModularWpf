using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace PsdFramework.ModularWpf.PopupWindows.Models;

public interface IValidatableDataContext
{
    IRelayCommand<DependencyObject> RegisterValidationContainerCommand { get; }
}