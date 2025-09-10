using System.Windows;

using PsdFramework.ModularWpf.ModularWindows.Models;

namespace PsdFramework.ModularWpf.ModularWindows;

internal static class Utils
{
    public static Task<object> DisplayWindow<TWindow>(this TWindow window, bool withResult = true)
        where TWindow : Window, IModularWindow
    {
        var tcs = new TaskCompletionSource<object>();

        window.Closed += (_, _) => tcs.SetResult(withResult ? window.GetResult() : null!);
        window.Show();
        
        return tcs.Task;
    }

    public static Task<TResult> DisplayWindow<TWindow, TResult>(this TWindow window)
        where TWindow : Window, IModularWindow<TResult>
    {
        var tcs = new TaskCompletionSource<TResult>();

        window.Closed += (_, _) => tcs.SetResult(window.GetResult());
        window.Show();

        return tcs.Task;
    }
}