using PsdFramework.ModularWpf.Interceptors;
using PsdFramework.ModularWpf.Popup.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;

namespace PsdFramework.ModularWpf.Popup.Service;

public interface IPopupService : IInterceptableService
{
    Task<PopupResult<TResult>> ShowPopupAsync<TPopup, TWindow, TResult>()
        where TPopup : class, IPopup<TWindow, TResult>
        where TWindow : class, IPopupWindow, new();

    Task<PopupResult<TResult>> ShowPopupAsync<TPopup, TWindow, TResult>(PopupOptions options)
        where TPopup : class, IPopup<TWindow, TResult>
        where TWindow : class, IPopupWindow, new();
}