using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.General;
using PsdFramework.ModularWpf.Navigation;
using PsdFramework.ModularWpf.Navigation.Abstract;
using PsdFramework.ModularWpf.Navigation.Navigatable;
using PsdFramework.ModularWpf.Navigation.NavigationHost;
using PsdFramework.ModularWpf.Navigation.Service;

namespace PsdFramework.ModularWpf.Tests.Unit.Navigation
{
    public sealed class NavigationCallSequenceTests
    {
        private readonly IServiceProvider _provider;
        private readonly INavigationService _navigationService;

        public NavigationCallSequenceTests()
        {
            _provider = new ServiceCollection()
                .AddComponents(ComponentOptions.Empty().WithConcreteTypes([typeof(MyNavigationHost), typeof(OldNavigatable), typeof(NewNavigatable)]))
                .AddSingleton<Queue<MethodCallInfo>>()
                .BuildServiceProvider();

            _navigationService = _provider.GetRequiredService<INavigationService>();
        }

        [Fact]
        public async Task NavigateAsync_ShouldHaveCorrectMethodCallSequence_WhenNavigatesSuccessfully()
        {
            var host = (INavigationHost)_provider.GetRequiredKeyedService(typeof(INavigationHost), typeof(MyNavigationHost));
            var queue = _provider.GetRequiredService<Queue<MethodCallInfo>>();

            await _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost(host).ToNavigatable<OldNavigatable>());
            await _navigationService.NavigateAsync(NavigationOptions.FromNavigationHost(host).ToNavigatable<NewNavigatable>());

            // first navigation
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(MyNavigationHost) && i.MethodName == nameof(INavigationHost.OnNavigatingAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(OldNavigatable) && i.MethodName == nameof(INavigatable.OnNavigatingToAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(OldNavigatable) && i.MethodName == nameof(INavigatable.OnNavigatedToAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(MyNavigationHost) && i.MethodName == nameof(INavigationHost.OnNavigatedAsync));

            // second navigation, containing the old navigation (to execute OnNavigatingFrom/OnNavigatedFrom)
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(MyNavigationHost) && i.MethodName == nameof(INavigationHost.OnNavigatingAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(OldNavigatable) && i.MethodName == nameof(INavigatable.OnNavigatingFromAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(NewNavigatable) && i.MethodName == nameof(INavigatable.OnNavigatingToAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(NewNavigatable) && i.MethodName == nameof(INavigatable.OnNavigatedToAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(OldNavigatable) && i.MethodName == nameof(INavigatable.OnNavigatedFromAsync));
            queue.Dequeue().Should().Match<MethodCallInfo>(i => i.ModelType == typeof(MyNavigationHost) && i.MethodName == nameof(INavigationHost.OnNavigatedAsync));

            queue.Count.Should().Be(0);
        }
    }
}

file record struct MethodCallInfo(Type ModelType, string MethodName);

[NavigationHost]
file sealed class MyNavigationHost : ObservableNavigationHostBase
{
    private readonly Queue<MethodCallInfo> _callSequence;

    public MyNavigationHost(Queue<MethodCallInfo> callSequence)
    {
        _callSequence = callSequence;
    }

    public override Task OnNavigatingAsync(NavigationContext context)
    {
        _callSequence.Enqueue(new(GetType(), nameof(OnNavigatingAsync)));
        return base.OnNavigatingAsync(context);
    }

    public override Task OnNavigatedAsync(NavigationContext context)
    {
        _callSequence.Enqueue(new(GetType(), nameof(OnNavigatedAsync)));
        return base.OnNavigatedAsync(context);
    }
}

file abstract class NavigatableBase : ObservableNavigatableBase
{
    private readonly Queue<MethodCallInfo> _callSequence;

    public NavigatableBase(Queue<MethodCallInfo> callSequence)
    {
        _callSequence = callSequence;
    }

    public override Task OnNavigatingFromAsync(NavigationContext context)
    {
        _callSequence.Enqueue(new(GetType(), nameof(OnNavigatingFromAsync)));
        return base.OnNavigatingFromAsync(context);
    }

    public override Task OnNavigatedFromAsync(NavigationContext context)
    {
        _callSequence.Enqueue(new(GetType(), nameof(OnNavigatedFromAsync)));
        return base.OnNavigatingFromAsync(context);
    }

    public override Task OnNavigatingToAsync(NavigationContext context)
    {
        _callSequence.Enqueue(new(GetType(), nameof(OnNavigatingToAsync)));
        return base.OnNavigatingToAsync(context);
    }

    public override Task OnNavigatedToAsync(NavigationContext context)
    {
        _callSequence.Enqueue(new(GetType(), nameof(OnNavigatedToAsync)));
        return base.OnNavigatingToAsync(context);
    }
}

[Navigatable]
file sealed class OldNavigatable(Queue<MethodCallInfo> queue) : NavigatableBase(queue);


[Navigatable]
file sealed class NewNavigatable(Queue<MethodCallInfo> queue) : NavigatableBase(queue);