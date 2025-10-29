using System.Windows;

using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.PopupWindows.Models.Result;

namespace PsdFramework.ModularWpf.PopupWindows.Models;

public interface IPopupComponentModel<TPopup, TResult> : IComponentModel
    where TPopup : Window, new()
{
    Task OnPopupOpened(ContextualParameters? parameters);
    Task OnPopupClosed();
    Task<PopupResult<TResult>> GetResult();
}