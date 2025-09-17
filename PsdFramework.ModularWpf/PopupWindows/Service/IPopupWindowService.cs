using System.Windows;

using PsdFramework.ModularWpf.PopupWindows.Models;
using PsdFramework.ModularWpf.PopupWindows.Models.Result;

namespace PsdFramework.ModularWpf.PopupWindows.Service;
public interface IPopupWindowService
{
    Task<PopupResult<TResult>> ShowPopup<TComponentModel, TPopup, TResult>()
        where TComponentModel : class, IPopupComponentModel<TPopup, TResult>
        where TPopup : Window, new();
    Task<PopupResult<TResult>> ShowPopup<TComponentModel, TPopup, TResult>(PopupOptions options)
        where TComponentModel : class, IPopupComponentModel<TPopup, TResult>
        where TPopup : Window, new();
}