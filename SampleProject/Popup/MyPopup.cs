using CommunityToolkit.Mvvm.Input;
using PsdFramework.ModularWpf.Popup.Abstract;
using PsdFramework.ModularWpf.Popup.Models;
using System.Diagnostics;

namespace SampleProject.Popup;

[Popup]
public sealed partial class MyPopup : ObservablePopupBase<MyPopupWindow, MyPopupResult>
{
    private readonly long _startTimestamp;

    public MyPopup()
    {
        _startTimestamp = Stopwatch.GetTimestamp();
    }

    [RelayCommand]
    private void OnClose()
    {
        base.SetResult(new MyPopupResult(TimeSpan.FromTicks(Stopwatch.GetTimestamp() - _startTimestamp)));
    }
}
