using Microsoft.Extensions.DependencyInjection;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;
using PsdFramework.ModularWpf.Parameters;

namespace PsdFramework.ModularWpf.Navigations.Service;

partial class NavigatorService
{
    // category & generic
    public Task NavigateTo<TNavigatable>(object category)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo<TNavigatable>(category);
    }

    public Task NavigateTo<TNavigatable>(object category, Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigatable : INavigatableComponentModel
    {
        return InternalNavigateTo(category, configureParameters);
    }

    private Task InternalNavigateTo<TNavigatable>(object category, Action<ParameterBuilder<TNavigatable>>? parameterBuilder = null)
        where TNavigatable : INavigatableComponentModel
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(typeof(TNavigatable));

        return NavigateWithParameters(navigation, navigatable, parameterBuilder);
    }

    // category & type
    public Task NavigateTo(object category, Type navigatableType)
    {
        return InternalNavigateTo(category, navigatableType);
    }

    public Task NavigateTo(object category, Type navigatableType, Action<ParameterBuilder> configureParameters)
    {
        return InternalNavigateTo(category, navigatableType, configureParameters);
    }

    private Task InternalNavigateTo(object category, Type navigatableType, Action<ParameterBuilder>? parameterBuilder = null)
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(navigatableType);

        return NavigateWithParameters(navigation, navigatable, parameterBuilder);
    }

    // category & instance
    public Task NavigateTo(object category, INavigatableComponentModel navigatable)
    {
        return InternalNavigateTo(category, navigatable);
    }

    public Task NavigateTo(object category, INavigatableComponentModel navigatable, Action<ParameterBuilder> configureParameters)
    {
        return InternalNavigateTo(category, navigatable, configureParameters);
    }

    private Task InternalNavigateTo(object category, INavigatableComponentModel navigatable, Action<ParameterBuilder>? parameterBuilder = null)
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);

        return NavigateWithParameters(navigation, navigatable, parameterBuilder);
    }
}