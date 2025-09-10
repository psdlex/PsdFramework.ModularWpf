using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.ModularWindows.Models;

namespace PsdFramework.ModularWpf.ModularWindows.Service;

public sealed class ModularWindowsWithDependenciesService : IModularWindowsWithDependenciesService
{
    private readonly IServiceProvider _serviceProvider;

    public ModularWindowsWithDependenciesService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task Show<TWindow>()
        where TWindow : Window, IModularWindow
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        return window.DisplayWindow(withResult: false);
    }

    public Task<object> ShowWithResult<TWindow>()
        where TWindow : Window, IModularWindow
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        return window.DisplayWindow();
    }

    public Task<TResult> ShowWithResult<TWindow, TResult>()
        where TWindow : Window, IModularWindow<TResult>
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        return window.DisplayWindow<TWindow, TResult>();
    }
}