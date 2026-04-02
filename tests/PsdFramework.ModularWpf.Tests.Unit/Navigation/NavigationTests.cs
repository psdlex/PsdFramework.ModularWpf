using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.Navigation;
using PsdFramework.ModularWpf.Navigation.Abstract;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;
using PsdFramework.ModularWpf.Navigation.Service;

namespace PsdFramework.ModularWpf.Tests.Unit.Navigation;

public sealed class NavigationTests
{
    private readonly IServiceProvider _provider;
    private readonly INavigationService _navigationService;

    public NavigationTests()
    {
        _provider = new ServiceCollection()
            .AddComponents()
            .AddNavigationService()
            .BuildServiceProvider();

        _navigationService = _provider.GetRequiredService<INavigationService>();
    }

    [Fact]
    public async Task NavigateAsync_ShouldSetNavigatableInNavigationHost_UsingNavigationHostInstanceAndNavigatableType_WhenNavigatesSuccesfully()
    {
        var host = (INavigationHost)_provider.GetRequiredKeyedService(typeof(INavigationHost), typeof(MyNavigationHost));
        await _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost(host).ToNavigatable<MyNavigatable>());

        host.CurrentModel.Should().BeOfType<MyNavigatable>();
    }

    [Fact]
    public async Task NavigateAsync_ShouldSetNavigatableInNavigationHost_UsingCategoryAndNavigatableType_WhenNavigatesSuccesfully()
    {
        var host = (INavigationHost)_provider.GetRequiredKeyedService(typeof(INavigationHost), typeof(MyNavigationHost));
        await _navigationService.NavigateAsync(NavigationOptions.FromCategory("default").ToNavigatable<MyNavigatable>());

        host.CurrentModel.Should().BeOfType<MyNavigatable>();
    }

    [Fact]
    public async Task NavigateAsync_ShouldSetNavigatableInNavigationHost_UsingNavigationHostInstanceAndNavigatableInstance_WhenNavigatesSuccesfully()
    {
        var host = (INavigationHost)_provider.GetRequiredKeyedService(typeof(INavigationHost), typeof(MyNavigationHost));
        var navigatable = (INavigatable)_provider.GetRequiredKeyedService(typeof(INavigatable), typeof(MyNavigatable));

        await _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost(host).ToNavigatable(navigatable));

        host.CurrentModel.Should().Be(navigatable);
    }
}

[NavigationHost(Category = "default", IsCached = true)]
file sealed class MyNavigationHost : ObservableNavigationHostBase;

[Navigatable]
file sealed class MyNavigatable : ObservableNavigatableBase;