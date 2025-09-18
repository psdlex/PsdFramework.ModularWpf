using System.Windows;

namespace PsdFramework.ModularWpf.PopupWindows.Service.Managers;

internal sealed class WindowStateManager
{
    private readonly Dictionary<Window, bool> _closingState = [];

    public void SetClosingState(Window window)
        => _closingState[window] = true;

    public bool IsClosing(Window window)
        => _closingState.TryGetValue(window, out var closing) && closing;

    public bool DisposeState(Window window)
        => _closingState.Remove(window);
}