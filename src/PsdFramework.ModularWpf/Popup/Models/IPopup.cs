using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;
using System.Windows;

namespace PsdFramework.ModularWpf.Popup.Models;

public interface IPopup<TWindow, TResult>
    where TWindow : IPopupWindow, new()
{
    Task OnOpenedAsync(ContextualParameters parameters);
    Task OnClosedAsync();

    Task<PopupResult<TResult>> GetResultAsync();
}