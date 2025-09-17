using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

using PsdFramework.ModularWpf.PopupWindows.Models;
using PsdFramework.ModularWpf.PopupWindows.Models.Result;

namespace PsdFramework.ModularWpf.PopupWindows.Abstract;

public abstract class ObservablePopupComponentModel<TPopup, TResult> : ObservableObject, IPopupComponentModel<TPopup, TResult>
    where TPopup : Window, new()
{
    private readonly TaskCompletionSource<PopupResult<TResult>> _completionSource;

    protected ObservablePopupComponentModel()
    {
        _completionSource = new();
    }

    public virtual void OnPopupExit()
    {
        _completionSource.TrySetResult(new PopupResult<TResult>(PopupExitBasis.Unexpected, default));
    }

    protected virtual void SetResult(TResult result)
    {
        _completionSource.TrySetResult(new PopupResult<TResult>(PopupExitBasis.Intentional, result));
    }

    public Task<PopupResult<TResult>> GetResult() => _completionSource.Task;
}