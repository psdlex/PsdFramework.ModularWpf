using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.Interceptors;
using PsdFramework.ModularWpf.Navigation;
using PsdFramework.ModularWpf.Navigation.Abstract;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;
using PsdFramework.ModularWpf.Navigation.Service;

namespace PsdFramework.ModularWpf.Tests.Unit.Navigation;

public sealed class NavigationInterceptionTests
{
    private readonly IServiceProvider _provider;
    private readonly INavigationService _navigationService;

    public NavigationInterceptionTests()
    {
        _provider = new ServiceCollection()
            .AddComponents(ComponentOptions.Empty().WithConcreteTypes([typeof(MyNavigationHost), typeof(MyNavigatable), typeof(NavigationInterceptor)]))
            .AddSingleton(new bool[2])
            .BuildServiceProvider();

        _navigationService = _provider.GetRequiredService<INavigationService>();
    }

    [Fact]
    public async Task NavigateAsync_ShouldInvokeIncerceptors_WhenTheyAreResolved()
    {
        var boolArray = _provider.GetRequiredService<bool[]>();
        await _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost<MyNavigationHost>().ToNavigatable<MyNavigatable>());

        boolArray[0].Should().BeTrue();
        boolArray[1].Should().BeTrue();
    }
}

[NavigationHost]
file sealed class MyNavigationHost : ObservableNavigationHostBase;

[Navigatable]
file sealed class MyNavigatable : ObservableNavigatableBase;

[Interceptor]
file sealed class NavigationInterceptor : InterceptorBase<INavigationService, NavigationContext>
{
    private readonly bool[] _boolArray;

    public NavigationInterceptor(bool[] boolArray)
    {
        _boolArray = boolArray;
    }

    public override Task InterceptPostExecutionAsync(NavigationContext context)
    {
        _boolArray[0] = true;
        return Task.CompletedTask;
    }

    public override Task InterceptPreExecutionAsync(NavigationContext context)
    {
        _boolArray[1] = true;
        return Task.CompletedTask;
    }
}