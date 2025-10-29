using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Models;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Service;

partial class NavigatorService
{
    // category & generic
    public Task NavigateTo<TNavigatable>(object category)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigatable>(category);
    }

    public Task NavigateTo<TNavigatable>(object category, Action<ContextualParameters> configureParameters)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigatable>(category, configureParameters);
    }

    private Task InternalNavigateTo<TNavigatable>(object category, Action<ContextualParameters>? configureParameters = null)
        where TNavigatable : INavigatableComponentModel
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }

    // category & type
    public Task NavigateTo(object category, Type navigatableType)
    {
        return InternalNavigateTo(category, navigatableType);
    }

    public Task NavigateTo(object category, Type navigatableType, Action<ContextualParameters> configureParameters)
    {
        return InternalNavigateTo(category, navigatableType, configureParameters);
    }

    private Task InternalNavigateTo(object category, Type navigatableType, Action<ContextualParameters>? configureParameters = null)
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(navigatableType);

        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }

    // category & instance
    public Task NavigateTo(object category, INavigatableComponentModel navigatable)
    {
        return InternalNavigateTo(category, navigatable);
    }

    public Task NavigateTo(object category, INavigatableComponentModel navigatable, Action<ContextualParameters> configureParameters)
    {
        return InternalNavigateTo(category, navigatable, configureParameters);
    }

    private Task InternalNavigateTo(object category, INavigatableComponentModel navigatable, Action<ContextualParameters>? configureParameters = null)
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);

        return NavigateWithParameters(navigation, navigatable, configureParameters);
    }
}