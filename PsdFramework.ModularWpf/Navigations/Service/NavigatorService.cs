using Microsoft.Extensions.DependencyInjection;

using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Service;

public sealed class NavigatorService : INavigatorService
{
    private readonly IServiceProvider _serviceProvider;

    public NavigatorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task NavigateTo<TNavigation, TNavigatable>()
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(typeof(TNavigation));
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return Navigate(navigation, navigatable);
    }

    public Task NavigateTo<TNavigatable>(object category)
        where TNavigatable : INavigatableComponentModel
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return Navigate(navigation, navigatable);
    }

    public Task NavigateTo(Type navigatableType, object category)
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(navigatableType);

        return Navigate(navigation, navigatable);
    }

    public Task NavigateTo<TNavigatable>(INavigationComponentModel navigation)
        where TNavigatable : INavigatableComponentModel
    {
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return Navigate(navigation, navigatable);
    }

    private async Task Navigate(INavigationComponentModel navigation, INavigatableComponentModel navigatable)
    {
        await navigation.OnPreNavigated(navigatable);

        await navigation.OnNavigated(navigatable);
        await navigatable.OnNavigated(navigation);
    }
}