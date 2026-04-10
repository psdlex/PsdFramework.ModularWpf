using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Popup.Abstract;
using PsdFramework.ModularWpf.Popup.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;
using PsdFramework.ModularWpf.Popup.Service;

namespace PsdFramework.ModularWpf.Tests.Unit.Popup;

public sealed class PopupTests
{
    private readonly IServiceProvider _provider;
    private readonly IPopupService _popupService;

    public PopupTests()
    {
        _provider = new ServiceCollection()
            .AddComponents(ComponentOptions.Empty().WithConcreteTypes([typeof(MyPopup), typeof(MyPopupCached), typeof(AutoClosingPopup)]))
            .AddSingleton(new List<Guid>(2))
            .BuildServiceProvider();

        _popupService = _provider.GetRequiredService<IPopupService>();
    }

    [Fact]
    public void ShowPopupAsync_ShouldAwaitAndReturnResult_WhenSetResultInvoked()
    {
        TestHelper.ExecuteOnStaThread(async () =>
        {
            var result = await _popupService.ShowPopupAsync<MyPopup, FakePopupWindow, string>();

            result.Value.Should().Be("result");
            result.ExitBasis.Should().Be(PopupExitBasis.Intentional);
        });
    }

    [Fact]
    public void ShowPopupAsync_ShouldAwaitAndReturnResult_WithReusedInstances_WhenSetResultInvoked()
    {
        TestHelper.ExecuteOnStaThread(async () =>
        {
            var guidList = _provider.GetRequiredService<List<Guid>>();

            await _popupService.ShowPopupAsync<MyPopupCached, FakePopupWindow, string>();
            await _popupService.ShowPopupAsync<MyPopupCached, FakePopupWindow, string>();

            guidList[0].Should().Be(guidList[1]);
        });
    }

    [Fact]
    public void ShowPopupAsync_ShouldReturnResult_WhenWindowIsClosedExternally()
    {
        TestHelper.ExecuteOnStaThread(async () =>
        {
            var result = await _popupService.ShowPopupAsync<AutoClosingPopup, AutoClosingPopupWindow, object?>();

            result.ExitBasis.Should().Be(PopupExitBasis.ExternalTermination);
        });
    }
}

[Popup]
file sealed class MyPopup : ObservablePopupBase<FakePopupWindow, string>
{
    public override async Task OnPopupOpenedAsync(Models.ContextualParameters parameters)
    {
        await Task.Delay(100);
        SetResult("result");
    }
}

[Popup(IsCached = true)]
file sealed class MyPopupCached : ObservablePopupBase<FakePopupWindow, string>
{
    private readonly List<Guid> _guidList;
    private readonly Guid _id = Guid.NewGuid();

    public MyPopupCached(List<Guid> guidList)
    {
        _guidList = guidList;
    }

    public override async Task OnPopupOpenedAsync(Models.ContextualParameters parameters)
    {
        _guidList.Add(_id);
        await Task.Delay(100);
        SetResult("result");
    }
}

[Popup]
file sealed class AutoClosingPopup : ObservablePopupBase<AutoClosingPopupWindow, object?>;

file sealed class AutoClosingPopupWindow : FakePopupWindow
{
    public AutoClosingPopupWindow()
    {
        Task.Run(async delegate
        {
            await Task.Delay(500);
            Close();
        });
    }
}