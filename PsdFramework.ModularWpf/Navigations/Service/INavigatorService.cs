using PsdFramework.ModularWpf.General.Models.Components;
using PsdFramework.ModularWpf.Navigations.Builder;
using PsdFramework.ModularWpf.Navigations.Models.Navigatable;
using PsdFramework.ModularWpf.Navigations.Models.Navigation;

namespace PsdFramework.ModularWpf.Navigations.Service;

public interface INavigatorService : IComponentModel
{
    // generic navigation
    Task NavigateTo<TNavigation, TNavigatable>()
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel;
    Task NavigateTo<TNavigation, TNavigatable>(Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigation : INavigationComponentModel
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo<TNavigation>(Type navigatableType)
        where TNavigation : INavigationComponentModel;
    Task NavigateTo<TNavigation>(Type navigatableType, Action<ParameterBuilder> configureParameters)
        where TNavigation : INavigationComponentModel;


    // category navigation
    Task NavigateTo<TNavigatable>(object category)
        where TNavigatable : INavigatableComponentModel;
    Task NavigateTo<TNavigatable>(object category, Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo(Type navigatableType, object category);
    Task NavigateTo(Type navigatableType, object category, Action<ParameterBuilder> configureParameters);


    // instance navigation
    Task NavigateTo<TNavigatable>(INavigationComponentModel navigation)
        where TNavigatable : INavigatableComponentModel;
    Task NavigateTo<TNavigatable>(INavigationComponentModel navigation, Action<ParameterBuilder<TNavigatable>> configureParameters)
        where TNavigatable : INavigatableComponentModel;

    Task NavigateTo(INavigationComponentModel navigation, Type navigatableType);
    Task NavigateTo(INavigationComponentModel navigation, Type navigatableType, Action<ParameterBuilder> configureParameters);


    // other
    IEnumerable<INavigatableComponentModel> GetAllNavigatables(object category);
    IEnumerable<INavigatableComponentModel> GetAllNavigatables(object category, Action<ParameterBuilder> configureParameters);
}