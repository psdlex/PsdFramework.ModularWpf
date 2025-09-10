using System.Diagnostics.CodeAnalysis;
using System.Windows;

using PsdFramework.ModularWpf.ModularWindows.Models;

namespace PsdFramework.ModularWpf.ModularWindows.Service;

public sealed class ModularWindowsService : IModularWindowsService
{
    private readonly IModularWindowsWithDependenciesService _modularWindowsWithDependencies;

    public ModularWindowsService(IModularWindowsWithDependenciesService modularWindowsWithDependencies)
    {
        _modularWindowsWithDependencies = modularWindowsWithDependencies;
    }

    public IModularWindowsWithDependenciesService WithDependencies() => _modularWindowsWithDependencies;

    public Task Show<TWindow>()
        where TWindow : Window, IModularWindow, new()
    {
        var window = new TWindow();
        return window.DisplayWindow(withResult: false);
    }

    public Task Show<TWindow>(object context)
        where TWindow : Window, IModularWindow
    {
        var window = Activator.CreateInstance(typeof(TWindow), context) as TWindow;

        if (window is null)
            ThrowWindowCreationException(typeof(TWindow));

        return window.DisplayWindow();
    }

    public Task<object> ShowWithResult<TWindow>()
        where TWindow : Window, IModularWindow, new()
    {
        var window = new TWindow();
        return window.DisplayWindow();
    }

    public Task<object> ShowWithResult<TWindow>(object context)
        where TWindow : Window, IModularWindow
    {
        var window = Activator.CreateInstance(typeof(TWindow), context) as TWindow;

        if (window is null)
            ThrowWindowCreationException(typeof(TWindow));

        return window.DisplayWindow();
    }

    public Task<TResult> ShowWithResult<TWindow, TResult>()
        where TWindow : Window, IModularWindow<TResult>, new()
    {
        var window = new TWindow();
        return window.DisplayWindow<TWindow, TResult>();
    }

    public Task<TResult> ShowWithResult<TWindow, TResult>(object context)
        where TWindow : Window, IModularWindow<TResult>, new()
    {
        var window = Activator.CreateInstance(typeof(TWindow), context) as TWindow;

        if (window is null)
            ThrowWindowCreationException(typeof(TWindow));

        return window.DisplayWindow<TWindow, TResult>();
    }

    [DoesNotReturn]
    private static void ThrowWindowCreationException(Type windowType) =>
        throw new InvalidOperationException($"Could not create an instance of the window type {windowType.FullName}. Ensure it has a constructor that accepts the provided context.");
}