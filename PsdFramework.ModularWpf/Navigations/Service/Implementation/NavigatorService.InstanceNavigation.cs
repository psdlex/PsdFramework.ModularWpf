using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;
using PsdFramework.ModularWpf.Parameters;

namespace PsdFramework.ModularWpf.Navigations.Service;

partial class NavigatorService
{
    // instance & generic
    public Task NavigateTo<TNavigatable>(INavigationComponentModel navigation)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigatable>(navigation);
    }

    public Task NavigateTo<TNavigatable>(INavigationComponentModel navigation, Action<ParameterBuilder<TNavigatable>> parameterBuilder)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo(navigation, parameterBuilder);
    }

    private Task InternalNavigateTo<TNavigatable>(INavigationComponentModel navigation, Action<ParameterBuilder<TNavigatable>>? parameterBuilder = null)
        where TNavigatable : INavigatableComponentModel
    {
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return NavigateWithParameters(navigation, navigatable, parameterBuilder);
    }

    // instance & type
    public Task NavigateTo(INavigationComponentModel navigation, Type navigatableType)
    {
        return InternalNavigateTo(navigation, navigatableType);
    }

    public  Task NavigateTo(INavigationComponentModel navigation, Type navigatableType, Action<ParameterBuilder> configureParameters)
    {
        return InternalNavigateTo(navigation, navigatableType, configureParameters);
    }

    private Task InternalNavigateTo(INavigationComponentModel navigation, Type navigatableType, Action<ParameterBuilder>? parameterBuilder = null)
    {
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(navigatableType);

        return NavigateWithParameters(navigation, navigatable, parameterBuilder);
    }

    // instance & instance
    public Task NavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable)
    {
        return InternalNavigateTo(navigation, navigatable);
    }

    public Task NavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ParameterBuilder> configureParameters)
    {
        return InternalNavigateTo(navigation, navigatable, configureParameters);
    }

    private Task InternalNavigateTo(INavigationComponentModel navigation, INavigatableComponentModel navigatable, Action<ParameterBuilder>? parameterBuilder = null)
    {
        return NavigateWithParameters(navigation, navigatable, parameterBuilder);
    }
}