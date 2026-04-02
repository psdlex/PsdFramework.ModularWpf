using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.Navigation;
using PsdFramework.ModularWpf.Navigation.Abstract;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;
using PsdFramework.ModularWpf.Navigation.Service;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace PsdFramework.ModularWpf.Tests.Unit.Navigation;

public sealed class NavigationCachingTests
{
    private readonly IServiceProvider _provider;
    private readonly INavigationService _navigationService;

    public NavigationCachingTests()
    {
        _provider = new ServiceCollection()
            .AddComponents()
            .AddNavigationService()
            .BuildServiceProvider();

        _navigationService = _provider.GetRequiredService<INavigationService>();
    }

    [Fact]
    public async Task NavigateAsync_ShouldReuseSameInstances_WhenCached()
    {
        var host = (INavigationHost)_provider.GetRequiredKeyedService(typeof(INavigationHost), typeof(MyNavigationHost));

        await _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost<MyNavigationHost>().ToNavigatable<MyNavigatable>());
        var firstNavigatable = host.CurrentModel;

        await _navigationService.NavigateAsync(NavigationOptions.FromCategory("default").ToNavigatable<MyNavigatable>());
        var secondNavigatable = host.CurrentModel;

        firstNavigatable.Should().NotBeNull();
        secondNavigatable.Should().NotBeNull();

        firstNavigatable.Should().Be(secondNavigatable);
    }
}

[NavigationHost(IsCached = true, Category = "default")]
file sealed class MyNavigationHost : ObservableNavigationHostBase;

[Navigatable(IsCached = true)]
file sealed class MyNavigatable : ObservableNavigatableBase;