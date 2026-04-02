using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Popup.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;
using System.Windows;

namespace PsdFramework.ModularWpf.Popup.Service;

internal sealed class PopupSession<TPopup, TWindow, TResult>
    where TPopup : class, IPopup<TWindow, TResult>
    where TWindow : IPopupWindow, new()
{
    private readonly TPopup _popup;
    private readonly TWindow _popupWindow;
    private readonly PopupOptions _options;

    private readonly Lock _lock = new();
    private bool _canClose = true;

    public PopupSession(TPopup popup, TWindow popupWindow, PopupOptions options)
    {
        _popup = popup;
        _popupWindow = popupWindow;
        _options = options;
    }

    public async Task<PopupResult<TResult>> ShowAsync(ContextualParameters parameters)
    {
        _popupWindow.Owner = _options.Owner ?? _popupWindow.Owner;
        _popupWindow.Closed += OnWindowClosed;

        if (_options.CloseOnDeactivation)
            _popupWindow.Deactivated += OnWindowDeactivated;

        var resultTask = _popup.GetResultAsync();
        _ = resultTask.ContinueWith(t => TryClose());

        _popupWindow.Show();

        await _popup.OnOpenedAsync(parameters);
        var result = await resultTask;
        await _popup.OnClosedAsync();

        return result;
    }

    private void OnWindowDeactivated(object? sender, EventArgs e) => TryClose();
    private async void OnWindowClosed(object? sender, EventArgs e)
    {
        _popupWindow.Closed -= OnWindowClosed;
        _popupWindow.Deactivated -= OnWindowDeactivated;

        await _popup.OnClosedAsync();
    }

    private void TryClose()
    {
        if (_lock.TryEnter(TimeSpan.FromSeconds(5)) == false)
            return;

        if (_canClose == false)
            return;

        _canClose = false;
        _popupWindow.Dispatcher.Invoke(() => _popupWindow.Close());

        _lock.Exit();
    }
}
