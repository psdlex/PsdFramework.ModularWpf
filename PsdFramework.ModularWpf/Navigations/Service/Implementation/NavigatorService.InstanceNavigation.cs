using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Service;

partial class NavigatorService
{
    // instance & generic
    public Task NavigateTo<TNavigatable>(INavigationComponentModel navigation)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigatable>(navigation);
    }

    public Task NavigateTo<TNavigatable>(INavigationComponentModel navigation, Action<ContextualParameters> configureParameters)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigatable>(navigation, configureParameters);
    }

    private Task InternalNavigateTo<TNavigatable>(INavigationComponentModel navigation, Action<ContextualParameters>? configureParameters = null)
        where TNavigatable : INavigatableComponentModel
    {
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }

    // instance & type
    public Task NavigateTo(INavigationComponentModel navigation, Type navigatableType)
    {
        return InternalNavigateTo(navigation, navigatableType);
    }

    public  Task NavigateTo(INavigationComponentModel navigation, Type navigatableType, Action<ContextualParameters> configureParameters)
    {
        return InternalNavigateTo(navigation, navigatableType, configureParameters);
    }

    private Task InternalNavigateTo(INavigationComponentModel navigation, Type navigatableType, Action<ContextualParameters>? configureParameters = null)
    {
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(navigatableType);

        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }

    // instance & instance
    public Task NavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable)
    {
        return InternalNavigateTo(navigation, navigatable);
    }

    public Task NavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ContextualParameters> configureParameters)
    {
        return InternalNavigateTo(navigation, navigatable, configureParameters);
    }

    private Task InternalNavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ContextualParameters>? configureParameters = null)
    {
        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }
}