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
    public Task NavigateTo(Type navigatableType, object category)
    {
        return InternalNavigateTo(navigatableType, category);
    }

    public Task NavigateTo(Type navigatableType, object category, Action<ParameterBuilder> configureParameters)
    {
        return InternalNavigateTo(navigatableType, category, configureParameters);
    }

    private Task InternalNavigateTo(Type navigatableType, object category, Action<ParameterBuilder>? parameterBuilder = null)
    {
        var navigation = _serviceProvider.GetRequiredKeyedService<INavigationComponentModel>(category);
        var navigatable = _serviceProvider.GetRequiredKeyedService<INavigatableComponentModel>(navigatableType);

        return NavigateWithParameters(navigation, navigatable, parameterBuilder);
    }
}