using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigation;
using PsdFramework.ModularWpf.Navigation.Abstract;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;
using PsdFramework.ModularWpf.Navigation.Service;
using PsdFramework.ModularWpf.Popup;
using PsdFramework.ModularWpf.Popup.Models;
using PsdFramework.ModularWpf.Popup.Models.Result;
using PsdFramework.ModularWpf.Popup.Service;
using PsdFramework.ModularWpf.View.Models;

namespace PsdFramework.ModularWpf.Tests.Unit.SharedComponentModel;

public sealed class SharedComponentTests
{
    private readonly IServiceProvider _provider;

    public SharedComponentTests()
    {
        _provider = new ServiceCollection()
            .AddComponents()
            .AddNavigationService()
            .AddPopupService()
            .BuildServiceProvider();
    }

    [Fact]
    public void RunAll_ShouldSynergizeAndShareSameInstance()
    {
        TestHelper.ExecuteOnStaThread(async () =>
        {
            var navigationService = _provider.GetRequiredService<INavigationService>();
            var popupService = _provider.GetRequiredService<IPopupService>();
            var navigationHost = (INavigationHost)_provider.GetRequiredKeyedService(typeof(INavigationHost), typeof(SharedModel));

            var view = _provider.GetRequiredService<IView<FakePopupWindow, SharedModel>>();
            var popupResult = await popupService.ShowPopupAsync<SharedModel, FakePopupWindow, SharedModel?>();
            await navigationService.NavigateAsync(NavigationOptions.FromNavigationHost<SharedModel>().ToNavigatable<SharedModel>());

            view.Model.Should().Be(navigationHost);
            navigationHost.CurrentModel.Should().Be(navigationHost);
            popupResult.Value.Should().Be(navigationHost);
        });
    }
}

[SharedComponentModel]
[NavigationHost(IsCached = true)]
[Navigatable(IsCached = true)]
[Popup(IsCached = true)]
[View<FakePopupWindow>(IsCached = true)]
file sealed class SharedModel : ObservableNavigationHostAndNavigatableBase, IPopup<FakePopupWindow, SharedModel?>
{
    public Task<PopupResult<SharedModel?>> GetResultAsync() => Task.FromResult(new PopupResult<SharedModel?>(PopupExitBasis.Intentional, this));
    public Task OnClosedAsync() => Task.CompletedTask;
    public Task OnOpenedAsync(ContextualParameters parameters) => Task.CompletedTask;
}