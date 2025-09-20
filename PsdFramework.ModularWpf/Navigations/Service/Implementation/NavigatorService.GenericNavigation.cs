using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Navigations.Builder;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Service;

partial class NavigatorService
{
    // generic & generic
    public Task NavigateTo<TNavigation, TNavigatable>()
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigation, TNavigatable>();
    }

    public Task NavigateTo<TNavigation, TNavigatable>(Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigation, TNavigatable>(configureParameters);
    }

    private Task InternalNavigateTo<TNavigation, TNavigatable>(Action<ParameterBuilder<TNavigatable>>? configureParameters = null)
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(typeof(TNavigation));
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }

    // generic & type
    public Task NavigateTo<TNavigation>(Type navigatableType)
        where TNavigation : INavigationComponentModel
    {
        return InternalNavigateTo<TNavigation>(navigatableType);
    }

    public Task NavigateTo<TNavigation>(Type navigatableType, Action<ParameterBuilder> configureParameters)
        where TNavigation : INavigationComponentModel
    {
        return InternalNavigateTo<TNavigation>(navigatableType, configureParameters);
    }

    private Task InternalNavigateTo<TNavigation>(Type navigatableType, Action<ParameterBuilder>? configureParameters = null)
        where TNavigation : INavigationComponentModel
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(typeof(TNavigation));
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(navigatableType);

        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }
}