using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;

namespace PsdFramework.ModularWpf.Popup.Models;

public interface IPopup<TWindow, TResult>
    where TWindow : IPopupWindow, new()
{
    Task OnPopupOpenedAsync(ContextualParameters parameters);
    Task OnPopupClosedAsync();

    Task<PopupResult<TResult>> GetResultAsync();
}