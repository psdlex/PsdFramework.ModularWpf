using System.Windows;

using PsdFramework.ModularWpf.ModularWindows.Models;

namespace PsdFramework.ModularWpf.ModularWindows.Service;
public interface IModularWindowsService
{
    Task Show<TWindow>() where TWindow : Window, IModularWindow, new();
    Task Show<TWindow>(object context) where TWindow : Window, IModularWindow;
    Task<TResult> ShowWithResult<TWindow, TResult>() where TWindow : Window, IModularWindow<TResult>, new();
    Task<TResult> ShowWithResult<TWindow, TResult>(object context) where TWindow : Window, IModularWindow<TResult>, new();
    Task<object> ShowWithResult<TWindow>() where TWindow : Window, IModularWindow, new();
    Task<object> ShowWithResult<TWindow>(object context) where TWindow : Window, IModularWindow;
    IModularWindowsWithDependenciesService WithDependencies();
}